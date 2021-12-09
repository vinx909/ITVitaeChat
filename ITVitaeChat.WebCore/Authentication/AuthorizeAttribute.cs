using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Interfaces;
using ITVitaeChat.WebCore.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITVitaeChat.WebCore.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Roles role;
        private readonly int groupId;
        private readonly IChatGroupService groupService;

        public AuthorizeAttribute(Roles role = Roles.user)
        {
            this.role = role;
        }

        public AuthorizeAttribute(int groupId, IChatGroupService groupService, Roles role = Roles.moderator)
        {
            this.role = role;
            this.groupId = groupId;
            this.groupService = groupService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Convert.ToInt64(userId) <= 0)
            {
                context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            //if (string.IsNullOrEmpty(roleClaim) || roleClaim != role)
            //{
            //    context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
            //    return;
            //}

            switch (role)
            {
                case Roles.user:
                    return;
                case Roles.moderator:
                    if( groupService.GetModeratedGroups(int.Parse(userId)).Result.Any(g => g.Id == groupId))
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                        return;
                    }
                case Roles.administrator:
                    if (roleClaim.Equals(Roles.administrator.ToString()))
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                        return;
                    }
            }
        }
    }
}
