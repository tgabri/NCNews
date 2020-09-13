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

namespace NCNews.API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with Articles
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
            try
            {
                _logger.LogInfo("Attempted to get all articles");
                var articles = await _articleRepository.FindAll();
                var response = _mapper.Map<IList<ArticleDTO>>(articles);
                _logger.LogInfo("Successfully got all articles");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
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
            try
            {
                _logger.LogInfo($"Attempted to get an article with id: {id}");
                var article = await _articleRepository.FindById(id);
                if (article == null)
                {
                    _logger.LogWarn($"Article with id: {id} was not found");
                    return NotFound();
                }
                var response = _mapper.Map<ArticleDTO>(article);
                _logger.LogInfo($"Successfully got article with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
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
            try
            {
                _logger.LogInfo($"Attempted to create an article");
                if (articleCreateDTO == null)
                {
                    _logger.LogWarn($"Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Article data was incomplete");
                    return BadRequest(ModelState);
                }
                var article = _mapper.Map<Article>(articleCreateDTO);
                var isSuccess = await _articleRepository.Create(article);
                if (!isSuccess)
                {
                    return InternalError($"Article creation failed");
                }
                _logger.LogInfo($"Article successfully created ");
                return Created("Create", new { article });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
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
            try
            {
                _logger.LogInfo($"Attempted to update an article with id: {id}");
                if (id < 1 || articleUpdateDTO == null || id != articleUpdateDTO.Id)
                {
                    _logger.LogWarn($"Article update failed with bad data");
                    return BadRequest();
                }
                var isExists = await _articleRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"Article with id: {id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Article data was incomplete");
                    return BadRequest();
                }
                var article = _mapper.Map<Article>(articleUpdateDTO);
                var isSuccess = await _articleRepository.Update(article);
                if (!isSuccess)
                {
                    return InternalError($"Article update failed");
                }
                _logger.LogInfo($"Article with id: {id} successfully updated ");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Delete article
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted to delete an article with id: {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"Article delete failed with bad data");
                    return BadRequest();
                }
                var isExists = await _articleRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"Article with id: {id} was not found");
                    return NotFound();
                }
                var article = await _articleRepository.FindById(id);
                var isSuccess = await _articleRepository.Delete(article);
                if (!isSuccess)
                {
                    return InternalError($"Article delete failed");
                }
                _logger.LogInfo($"Article with id: {id} successfully deleted ");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        private IActionResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the Admin");
        }
    }
}
