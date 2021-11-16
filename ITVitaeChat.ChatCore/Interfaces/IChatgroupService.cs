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
        public Task<bool> Create(string name, int maxUser, ChatgroupVisibility visibility, string password, uint userId);
        public Task<bool> Create(string name, ChatgroupVisibility visibility, string password, uint userId);
        public Task<bool> Create(string name, int maxUser, uint userId);
        public Task<bool> Create(string name, uint userId);
        public Task<bool> Exist(uint groupId);
    }
}
