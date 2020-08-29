using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCNews.API.Contracts;
using NCNews.API.Models;

namespace NCNews.API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with Articles
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILoggerService _logger;

        public ArticlesController(IArticleRepository articleRepository, ILoggerService logger)
        {
            _articleRepository = articleRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
