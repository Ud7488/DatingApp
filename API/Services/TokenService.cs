using API.Interface;
using API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

         public string CreateToken(AppUser user)
         {
             var claims = new List<Claim>
             {
                 new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
             };

             var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 Subject = new ClaimsIdentity(claims),
                 Expires = DateTime.Now.AddDays(7),
                 SigningCredentials = creds
             };

             var tokenHandeler = new JwtSecurityTokenHandler();

            var token = tokenHandeler.CreateToken(tokenDescriptor);

            return tokenHandeler.WriteToken(token);

        }
    }
}