using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCNews.API.Contracts;
using NCNews.API.DTOs;
using NCNews.API.Models;
using NCNews.API.Statics;

namespace NCNews.API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with Topics table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public TopicsController(ITopicRepository topicRepository, ILoggerService logger, IMapper mapper)
        {
            _topicRepository = topicRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Topics
        /// </summary>
        /// <returns>List of Topics</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopics()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToGet(location));
                var topics = await _topicRepository.FindAll();
                var response = _mapper.Map<IList<TopicDTO>>(topics);
                _logger.LogInfo(LogMessages.Success(location));

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Get a Topic by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Book</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopic(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToGet(location, id));
                var topic = await _topicRepository.FindById(id);
                if (topic == null)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                var response = _mapper.Map<TopicDTO>(topic);
                _logger.LogInfo(LogMessages.Success(location, id));
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, id, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Create topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>Topic object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TopicCreateDTO topicCreateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToCreate(location));
                if (topicCreateDTO == null)
                {
                    _logger.LogWarn(LogMessages.EmptyRequest(location));
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn(LogMessages.IncompleteData(location));
                    return BadRequest(ModelState);
                }
                var topic = _mapper.Map<Topic>(topicCreateDTO);
                var isSuccess = await _topicRepository.Create(topic);
                if (!isSuccess)
                {
                    return InternalError(LogMessages.CreateFailed(location));
                }
                _logger.LogInfo(LogMessages.Success(location));
                _logger.LogInfo($"{topic}");
                return Created("Create", new { topic });
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Update topic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] TopicUpdateDTO topicUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToUpdate(location, id));
                if (id < 1 || topicUpdateDTO == null || id != topicUpdateDTO.Id)
                {
                    _logger.LogWarn(LogMessages.BadData(location, id));
                    return BadRequest();
                }
                var isExists = await _topicRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn(LogMessages.IncompleteData(location, id));
                    return BadRequest(ModelState);
                }
                var topic = _mapper.Map<Topic>(topicUpdateDTO);
                var isSuccess = await _topicRepository.Update(topic);
                if (!isSuccess)
                {
                    return InternalError(LogMessages.UpdateFailed(location, id));
                }
                _logger.LogInfo(LogMessages.Success(location, id));
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Delete topic by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToDelete(location, id));
                if (id < 1)
                {
                    _logger.LogWarn(LogMessages.BadData(location, id));
                    return BadRequest();
                }
                var isExists = await _topicRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                var topic = await _topicRepository.FindById(id);
                var isSuccess = await _topicRepository.Delete(topic);
                if (!isSuccess)
                {
                    return InternalError(LogMessages.DeleteFailed(location, id));
                }
                _logger.LogInfo(LogMessages.Success(location, id));
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller}/{action}";
        }

        private IActionResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the Admin");
        }
    }
}
