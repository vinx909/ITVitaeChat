using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.WebCore.Interfaces;
using ITVitaeChat.WebCore.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ITVitaeChat.WebCore.Services
{
    public class JwtService : IJwtService
    {
        private readonly AppSettings appSettings;

        public JwtService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public string Generate(List<Claim> claims)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SigninKey));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = appSettings.Issuer,
                Audience = appSettings.Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
