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
using ITVitaeChat.ChatCore.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using AuthorizeAttribute = ITVitaeChat.WebCore.Authentication.AuthorizeAttribute;

namespace ITVitaeChat.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Groups : ControllerBase
    {
        private readonly IChatGroupService groupService;
        private readonly IChatGroupUserService groupUserService;

        public Groups(IChatGroupService groupService, IChatGroupUserService groupUserService)
        {
            this.groupService = groupService;
            this.groupUserService = groupUserService;
        }

        [HttpPost]
        public async Task AddGroup(string name, int maxusers, ChatGroupVisibility visibility, string password)
        {
            //TODO get userId from token
            //if (int.TryParse(context.HttpContext.User.Identity.Name, out int userId))
            //{
                int userId = 1;
                await groupService.Create(name, maxusers, visibility, password, userId, groupUserService);
            //}
        }

        [HttpDelete("remove")]
        public async Task RemoveUserFromGroup(int userId, int groupId)
        {
            await groupUserService.Remove(groupId, userId);
        }

        [HttpDelete("{id}/remove")]
        public async Task RemoveGroup([FromRoute] int id)
        {
            await groupService.Remove(id, groupUserService);
        }
    }
}
