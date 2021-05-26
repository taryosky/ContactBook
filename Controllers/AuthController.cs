using ContactBook.DTO;
using ContactBook.Models;
using ContactBook.Utilities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    //[Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signinManager;
        private readonly UserManager<User> _userManager;
        private IConfiguration _config;
        public readonly RoleManager<User> _roleManager;

        public AuthController(SignInManager<User> signinManager, UserManager<User> userManager, IConfiguration config)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost]
        [Route("[controller]/Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (string.IsNullOrWhiteSpace(loginDetails.Email) || string.IsNullOrWhiteSpace(loginDetails.Password))
            {
                return BadRequest();
            }

            //fetch user
            var user = await _userManager.FindByEmailAsync(loginDetails.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDetails.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName),
                };

                foreach (var role in userRoles)
                {
                    var claim = new Claim(ClaimTypes.Role, role);
                    claims.Add(claim);
                }

                var token = TokenGenerator.GenerateToken(claims, _config, user);
                await Task.CompletedTask;
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
