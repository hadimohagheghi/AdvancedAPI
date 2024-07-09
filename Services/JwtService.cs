using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public class JwtService:IJwtService
    {

        public string Generate(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes("LongerThan-16Char-SecretKey");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256);
            var claims = _getClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "MyWebSite",
                Audience = "MyWebSite.",
                IssuedAt = DateTime.Now,
                
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);

            return jwt;
        }

        private IEnumerable<Claim> _getClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone,"09017915622")

            };


            var roles = new Role[] { new Role { Name = "Admin" } };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role.Name));
            }

            return claims;
        }

    }
}
