using ITVitaeChat.WebCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.WebCore.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private AppSettings appSettings;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;

            // Bearer eygjrie3gjh893g9orehuogierhighokf
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.SigninKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,

                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
                var role = jwtToken.Claims.First(x => x.Type == "role").Value;

                if (context.User != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    };

                    ClaimsIdentity appIdentity = new(claims);
                    context.User.AddIdentity(appIdentity);
                }
            }
            catch (Exception)
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
