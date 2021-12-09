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
using ITVitaeChat.WebCore.Interfaces;
using ITVitaeChat.WebCore.Enums;
using AuthorizeAttribute = ITVitaeChat.WebCore.Authentication.AuthorizeAttribute;

namespace ITVitaeChat.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Users : ControllerBase
    {
        private readonly IAdministratorService administratorService;
        private readonly IChatGroupService groupService;
        private readonly IChatGroupUserService groupUserService;
        private readonly IUserService userService;
        private readonly IJwtService jwtService;

        public Users(IAdministratorService administratorService, IChatGroupService groupService, IChatGroupUserService groupUserService, IUserService userService, IJwtService jwtService)
        {
            this.administratorService = administratorService;
            this.groupService = groupService;
            this.userService = userService;
            this.groupUserService = groupUserService;
            this.jwtService = jwtService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string name, string username, string emailaddress, string password)
        {
            if (await userService.Register(name, username, emailaddress, password, groupUserService))
            {
                return Accepted();
            }
            else
            {
                return BadRequest();
                //TODO add specific reactions for specific reasons it wasn't added
            }
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            (ChatCore.Enums.LoginResult, User) loginAttemp = await userService.Login(username, password);

            switch (loginAttemp.Item1)
            {
                case ChatCore.Enums.LoginResult.loggedIn:
                    User user = loginAttemp.Item2;

                    var role = GetRole(user);

                    List<Claim> claims = new()
                    {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.Name, user.DisplayName),
                        new(ClaimTypes.Email, user.Emailadres),
                        new(ClaimTypes.Role, (await role).ToString())
                    };

                    return Ok(new
                    {
                        token = jwtService.Generate(claims)
                    });
                case ChatCore.Enums.LoginResult.blocked:
                    return Forbid();
                case ChatCore.Enums.LoginResult.notValidated:
                    return UnprocessableEntity();
                case ChatCore.Enums.LoginResult.usernameNotFound:
                    return NotFound();
                case ChatCore.Enums.LoginResult.incorrectPassword:
                    return Unauthorized();
                case ChatCore.Enums.LoginResult.incompleteData:
                    return BadRequest();
            }
            throw new NotImplementedException();
        }


        //TODO incorrect return but currently unsure exactly how to check for who's online (should be simple with identity but have no clue how that works) nor exactly sure what the return should be.
        [HttpGet("list")]
        [Authorize(Roles.user)]
        public async Task<ActionResult<IEnumerable<User>>> LoggedinUsers()
        {
            throw new NotImplementedException();
        }

        [HttpGet("block/{id}")]
        [Authorize(Roles.administrator)]
        public async Task<IActionResult> Block([FromRoute] int id)
        {
            if(await userService.Block(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task Edit(int id, string username, string password, string emailadres)
        {
            await userService.Edit(id, username, password, emailadres);
        } 

        private async Task<Roles> GetRole(User user)
        {
            if((await administratorService.Get(user.Id)) != null)
            {
                return Roles.administrator;
            }
            if((await groupService.GetModeratedGroups(user.Id)).Any())
            {
                return Roles.moderator;
            }
            return Roles.user;
        }
    }
}
