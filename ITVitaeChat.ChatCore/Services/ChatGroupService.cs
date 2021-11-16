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
        private const ChatgroupVisibility defaultVisibility = ChatgroupVisibility.Public;

        private readonly IRepository<Chatgroup> chatgroupRepository;
        private readonly IChatgroupUserService chatgroupUserService;
        private readonly IHashAndSaltService hashAndSaltService;
        private readonly IUserService userService;

        public ChatGroupService(IRepository<Chatgroup> chatgroupRepository, IChatgroupUserService chatgroupUserService, IHashAndSaltService hashAndSaltService, IUserService userService)
        {
            this.chatgroupRepository = chatgroupRepository;
            this.chatgroupUserService = chatgroupUserService;
            this.hashAndSaltService = hashAndSaltService;
            this.userService = userService;
        }

        public async Task<bool> Create(string name, int maxUser, ChatgroupVisibility visibility, string password, uint userId)
        {
            if (await userService.Exists(userId) && !string.IsNullOrWhiteSpace(name))
            {
                Chatgroup group = new() { Name = name, MaxUsers = maxUser, ModeratorId = userId };
                if(visibility == ChatgroupVisibility.Public)
                {
                    group.Visibility = visibility;
                    group.PasswordSalt = null;
                    group.Password = null;
                }
                else if(visibility == ChatgroupVisibility.Private)
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
                uint? groupId = await chatgroupRepository.Add(group);
                if(groupId == null)
                {
                    throw new Exception(string.Format(exceptionNoGroupIdRecoveredMessage, name));
                }
                chatgroupUserService.Add((uint)groupId, userId);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public async Task<bool> Create(string name, ChatgroupVisibility visibility, string password, uint userId)
        {
            return await Create(name, defaultMaxUser, visibility, password, userId);
        }
        
        public async Task<bool> Create(string name, int maxUser, uint userId)
        {
            return await Create(name, maxUser, defaultVisibility, null, userId);
        }

        public async Task<bool> Create(string name, uint userId)
        {
            return await Create(name, defaultMaxUser, defaultVisibility, null, userId);
        }

        public Task<bool> Exist(uint groupId)
        {
            throw new NotImplementedException();
        }
    }
}
