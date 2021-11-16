using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class ChatGroupUserService : IChatgroupUserService
    {
        private readonly IRepository<ChatgroupUser> groupUserRepository;
        private readonly IChatGroupService groupService;
        private readonly IUserService userService;

        public ChatGroupUserService(IRepository<ChatgroupUser> groupUserRepository, IChatGroupService groupService, IUserService userService)
        {
            this.groupUserRepository = groupUserRepository;
            this.groupService = groupService;
            this.userService = userService;
        }

        public async Task<bool> Add(uint groupId, uint userId)
        {
            if(await groupService.Exist(groupId) && await userService.Exists(userId) && !await Exists(groupId, userId))
            {
                await groupUserRepository.Add(new() { ChatgroupId = groupId, UserId = userId });
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Exists(uint groupId, uint senderId)
        {
            return await groupUserRepository.Contains(g => g.ChatgroupId.Equals(groupId) && g.UserId.Equals(senderId));
        }
    }
}
