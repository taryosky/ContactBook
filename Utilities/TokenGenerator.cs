using ContactBook.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactBook.Utilities
{
    public class TokenGenerator
    {
        public static object GenerateToken(IEnumerable<Claim> claims, IConfiguration config, User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTKey:JWTSecurityKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new
            {
                token = tokenHandler.WriteToken(token),
                UserId = user.Id
            };
        }
    }
}
