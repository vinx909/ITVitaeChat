using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class ChatGroupService : IChatGroupService
    {
        private const string exceptionNoGroupIdRecoveredMessage = "after the addition of group {0} the repository failed to return to return an id (at least origionaly done with reflection in a generic repository and could be solved with a entity specific repository)";
        private const int defaultMaxUser = 0;
        private const ChatGroupVisibility defaultVisibility = ChatGroupVisibility.Public;

        private readonly IRepository<ChatGroup> chatgroupRepository;
        private readonly IHashAndSaltService hashAndSaltService;
        private readonly IUserService userService;

        public ChatGroupService(IRepository<ChatGroup> chatgroupRepository, IHashAndSaltService hashAndSaltService, IUserService userService)
        {
            this.chatgroupRepository = chatgroupRepository;
            this.hashAndSaltService = hashAndSaltService;
            this.userService = userService;
        }

        public async Task<bool> Create(string name, int maxUser, ChatGroupVisibility visibility, string password, int userId, IChatGroupUserService chatgroupUserService)
        {
            if (await userService.Exists(userId) && !string.IsNullOrWhiteSpace(name))
            {
                ChatGroup group = new() { Name = name, MaxUsers = maxUser, ModeratorId = userId };
                if(visibility == ChatGroupVisibility.Public)
                {
                    group.Visibility = visibility;
                    group.PasswordSalt = null;
                    group.Password = null;
                }
                else if(visibility == ChatGroupVisibility.Private)
                {
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        group.Visibility = visibility;
                        group.PasswordSalt = hashAndSaltService.GenerateSalt();
                        group.Password = hashAndSaltService.Hash(password, group.PasswordSalt);
                    }
                    else
                    {
                        return false;
                    }
                }
                int? groupId = await chatgroupRepository.Add(group);
                if(groupId == null)
                {
                    throw new Exception(string.Format(exceptionNoGroupIdRecoveredMessage, name));
                }
                if(chatgroupUserService != null)
                {
                    await chatgroupUserService.Add((int)groupId, userId);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public async Task<bool> Create(string name, ChatGroupVisibility visibility, string password, int userId, IChatGroupUserService chatgroupUserService)
        {
            return await Create(name, defaultMaxUser, visibility, password, userId, chatgroupUserService);
        }
        
        public async Task<bool> Create(string name, int maxUser, int userId, IChatGroupUserService chatgroupUserService)
        {
            return await Create(name, maxUser, defaultVisibility, null, userId, chatgroupUserService);
        }

        public async Task<bool> Create(string name, int userId, IChatGroupUserService chatgroupUserService)
        {
            return await Create(name, defaultMaxUser, defaultVisibility, null, userId, chatgroupUserService);
        }

        public async Task<bool> Exists(int groupId)
        {
            return await chatgroupRepository.Contains(groupId);
        }

        public async Task<bool> Remove(int groupId, IChatGroupUserService groupUserService = null)
        {
            ChatGroup group = await chatgroupRepository.Get(groupId);
            if(group == null)
            {
                return false;
            }
            if(groupUserService != null)
            {
                IEnumerable<int> userIds = await groupUserService.GetGroupUsers(groupId);
                List<Task> tasks = new();
                foreach (int userId in userIds)
                {
                    tasks.Add(groupUserService.Remove(groupId, userId));
                }
                Task.WaitAll(tasks.ToArray());
            }
            await chatgroupRepository.Delete(group);
            return true;
        }
    }
}
