using Microsoft.AspNetCore.Mvc;
using ZingTickets.API.Controllers;

namespace ZingTickets.MovieManagement.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MoviesController : BaseController
    {
        private readonly IConfiguration _configuration;

        public MoviesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult Movies()
        {
            return Ok("This is Version 2 of Movies API");
        }


        //[HttpGet("{id}")]
        //public IActionResult GetMovie(string id)
        //{
        //    return Ok();
        //}


        //[HttpPost]
        //public IActionResult AddMovie()
        //{
        //    return Ok();
        //}

        //[HttpPut]
        //public IActionResult UpdateMovie()
        //{
        //    return Ok();
        //}

        //[HttpDelete]
        //public IActionResult DeleteMovie()
        //{
        //    return Ok();
        //}
    }
}
