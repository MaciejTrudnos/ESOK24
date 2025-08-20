using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace esok.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("Check")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
