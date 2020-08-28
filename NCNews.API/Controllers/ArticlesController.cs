using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCNews.API.Models;

namespace NCNews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {


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
