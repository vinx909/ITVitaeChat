using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class ChatGroupUserService : IChatGroupUserService
    {
        private readonly IRepository<ChatGroupUser> groupUserRepository;
        private readonly IChatGroupService groupService;
        private readonly IUserService userService;

        public ChatGroupUserService(IRepository<ChatGroupUser> groupUserRepository, IChatGroupService groupService, IUserService userService)
        {
            this.groupUserRepository = groupUserRepository;
            this.groupService = groupService;
            this.userService = userService;
        }

        public async Task<bool> Add(int groupId, int userId)
        {
            if(await groupService.Exists(groupId) && await userService.Exists(userId) && !await Exists(groupId, userId))
            {
                await groupUserRepository.Add(new() { ChatgroupId = groupId, UserId = userId });
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Exists(int groupId, int userId)
        {
            return await groupUserRepository.Contains(MatchGroupUser(groupId, userId));
        }

        public async Task<bool> Remove(int groupid, int userId)
        {
            ChatGroupUser groupUser = await groupUserRepository.Get(MatchGroupUser(groupid, userId));
            if(groupUser != null)
            {
                await groupUserRepository.Delete(groupUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<int>> GetGroupUsers(int groupId)
        {
            IEnumerable<ChatGroupUser> groupUsers = groupUserRepository.GetAll(g => g.ChatgroupId.Equals(groupId)).Result;
            List<int> userIds = new();
            foreach (ChatGroupUser groupUser in groupUsers)
            {
                userIds.Add(groupUser.UserId);
            }
            return userIds;
        }

        private static Expression<Func<ChatGroupUser, bool>> MatchGroupUser(int groupId, int userId)
        {
            return g => g.ChatgroupId.Equals(groupId) && g.UserId.Equals(userId);
        }
    }
}
