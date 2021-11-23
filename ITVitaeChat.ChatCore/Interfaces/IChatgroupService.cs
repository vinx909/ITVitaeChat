using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Enums;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatGroupService
    {
        public Task<bool> Create(string name, int maxUser, ChatGroupVisibility visibility, string password, int userId, IChatGroupUserService chatgroupUserService = null);
        public Task<bool> Create(string name, ChatGroupVisibility visibility, string password, int userId, IChatGroupUserService chatgroupUserService = null);
        public Task<bool> Create(string name, int maxUser, int userId, IChatGroupUserService chatgroupUserService = null);
        public Task<bool> Create(string name, int userId, IChatGroupUserService groupUserService = null);
        public Task<bool> Exists(int groupId);
        public Task<bool> Remove(int groupId, IChatGroupUserService groupUserService = null);
    }
}
