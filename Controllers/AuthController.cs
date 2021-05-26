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
    [ApiController]
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
            if (loginDetails == null || string.IsNullOrWhiteSpace(loginDetails.Email) || string.IsNullOrWhiteSpace(loginDetails.Password))
            {
                return BadRequest();
            }

            //fetch user
            var user = await _userManager.FindByEmailAsync(loginDetails.Email);

            //return unauthorize if user from database is null
            if (user == null) return Unauthorized();

            //fetch user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            var signinResult = await _signinManager.PasswordSignInAsync(user, loginDetails.Password, false, false);
            if (!signinResult.Succeeded) return Unauthorized();

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName)
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
    }
}
