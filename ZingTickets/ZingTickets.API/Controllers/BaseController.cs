using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ZingTickets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected JsonResult PackageData<T>(T data, HttpStatusCode httpStatusCode)
        {
            var result = Json(data);
            result.StatusCode = (int)httpStatusCode;
            return result;
        }
    }
}
