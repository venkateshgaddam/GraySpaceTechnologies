using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZingTickets.API.Controllers;

namespace ZingTickets.MovieManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : BaseController
    {
        [HttpGet]
        public IActionResult GetActionResult()
        {
            return Ok("It's Working");
        }
    }
}
