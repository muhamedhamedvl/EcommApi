using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Identity;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.InfraStructure.Repositores.Service
{
    public class GenrateToken : IGenrateToken
    {
        private readonly IConfiguration configuration;
        public GenrateToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateTokenAsync(AppUser appUser)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email)
            };

            var Security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(Security);

            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = configuration["Token:Issuer"],
                SigningCredentials = credentials,
                NotBefore = DateTime.Now
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
