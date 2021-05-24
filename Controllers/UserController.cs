using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("[controller]/All-users")]
        public async Task<IActionResult> GetAlloUsers([FromQuery] string page)
        {
            await Task.CompletedTask;
            return Ok("Retrieved all users");
        }

        [HttpGet]
        [Route("[controller]/{IdOrEmail}")]
        public async Task<IActionResult> GetUser([FromRoute] string IdOREmail)
        {
            await Task.CompletedTask;
            return Ok(IdOREmail);
        }

        [HttpGet]
        [Route("[controller]/search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            await Task.CompletedTask;
            return Ok(query);
        }

        [HttpPost]
        [Route("[controller]/add-new")]
        public async Task<IActionResult> AddUser([FromForm] string UserDTO)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpDelete]
        [Route("[controller]/delete/{Id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string Id)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPut]
        [Route("[controller]/update/{Id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string Id, [FromForm] string UserToUpdateDTO)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPatch]
        [Route("[controller]/upload/{UserId}")]
        public async Task<IActionResult> UploadPhoto([FromRoute] string UserId, [FromForm] IFormFile photo)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
