using Microsoft.AspNetCore.Mvc;
using ZingTickets.API.Controllers;

namespace ZingTickets.MovieManagement.Controllers.V1
{
    [ApiVersion("1.0")]
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
        [MapToApiVersion("1.0")]
        public IActionResult Movies()
        {
            var result = _configuration["/grayspace/aws/dev/userpoolid"];
            return Ok(result);
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
