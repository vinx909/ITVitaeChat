using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using ITVitaeChat.ChatCore.Interfaces;
using ITVitaeChat.ChatCore.Entities;
using System.Security.Claims;

namespace ITVitaeChat.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Users : ControllerBase
    {
        private readonly IChatGroupUserService groupUserService;
        private readonly IUserService userService;

        public Users(IChatGroupUserService groupUserService, IUserService userService)
        {
            this.userService = userService;
            this.groupUserService = groupUserService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Register(string name, string username, string emailaddress, string password)
        {
            return await userService.Register(name, username, emailaddress, password, groupUserService);
        }

        //TODO incorrect return but currently no clue how to send a token
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            User user = (await userService.Login(username, password)).Item2;
            if(user == null)
            {
                return Unauthorized();
            }

            List<Claim> claims = new()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Email, user.Emailadres)
            };

            //var identity = new ClaimsIdentity(claims)
            //return user;
            throw new NotImplementedException();
        }

        //TODO incorrect return but currently unsure exactly how to check for who's online (should be simple with identity but have no clue how that works) nor exactly sure what the return should be.
        [HttpGet("list")]
        public async Task<IEnumerable<User>> LoggedinUsers()
        {
            throw new NotImplementedException();
        }

        [HttpGet("block/{id}")]
        public async Task Block([FromRoute] int id)
        {
            await userService.Block(id);
        }

        [HttpPut]
        public async Task Edit(int id, string username, string password, string emailadres)
        {
            await userService.Edit(id, username, password, emailadres);
        }
    }
}
