using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("[controller]/Login")]
        public async Task<IActionResult> Login([FromForm] string Username, [FromForm] string Password)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
