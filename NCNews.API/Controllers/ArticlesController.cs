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
    /// Endpoint used to interact with Articles table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleRepository articleRepository, ILoggerService logger, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Articles
        /// </summary>
        /// <returns>List of Articles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticles()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToGet(location));
                var articles = await _articleRepository.FindAll();
                var response = _mapper.Map<IList<ArticleDTO>>(articles);
                _logger.LogInfo(LogMessages.Success(location));
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Get an article by Id
        /// </summary>
        /// <returns>An Article</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticle(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToGet(location, id));
                var article = await _articleRepository.FindById(id);
                if (article == null)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                var response = _mapper.Map<ArticleDTO>(article);
                _logger.LogInfo(LogMessages.Success(location, id));
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, id, e.Message, e.InnerException));
            }
        }

        /// <summary>
        /// Create article
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ArticleCreateDTO articleCreateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToCreate(location));
                if (articleCreateDTO == null)
                {
                    _logger.LogWarn(LogMessages.EmptyRequest(location));
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn(LogMessages.IncompleteData(location));
                    return BadRequest(ModelState);
                }
                var article = _mapper.Map<Article>(articleCreateDTO);
                var isSuccess = await _articleRepository.Create(article);
                if (!isSuccess)
                {
                    return InternalError(LogMessages.CreateFailed(location));
                }
                _logger.LogInfo(LogMessages.Success(location));
                return Created("Create", new { article });
            }
            catch (Exception e)
            {
                return InternalError(LogMessages.InternalError(location, e.Message, e.InnerException));
            }
        }


        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="id"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(LogMessages.AttemptedToUpdate(location, id));
                if (id < 1 || articleUpdateDTO == null || id != articleUpdateDTO.Id)
                {
                    _logger.LogWarn(LogMessages.BadData(location, id));
                    return BadRequest();
                }
                var isExists = await _articleRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn(LogMessages.IncompleteData(location, id));
                    return BadRequest();
                }
                var article = _mapper.Map<Article>(articleUpdateDTO);
                var isSuccess = await _articleRepository.Update(article);
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
        /// Delete article
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
                var isExists = await _articleRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn(LogMessages.NotFound(location, id));
                    return NotFound();
                }
                var article = await _articleRepository.FindById(id);
                var isSuccess = await _articleRepository.Delete(article);
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
