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

namespace ITVitaeChat.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Messages : ControllerBase
    {
        private readonly IChatMessageService messageService;

        public Messages(IChatMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        public async Task SendMessage(string message, int senderId, int groupId)
        {
            await messageService.Add(message, senderId, groupId);
        }

        [HttpGet("{page}&{groupId}")]
        public async Task<IEnumerable<ChatMessage>> GetMessages([FromRoute] int page, [FromRoute] int groupId)
        {
            return await messageService.Get(groupId, page);
        }

        [HttpGet("({page}&{groupId})({query})")]
        public async Task<IEnumerable<ChatMessage>> FindMessages([FromRoute] int page, [FromRoute] int groupId, [FromRoute] string query)
        {
            return await messageService.Get(groupId, query, page);
        }
    }
}
