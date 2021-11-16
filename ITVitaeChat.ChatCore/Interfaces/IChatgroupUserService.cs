using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatgroupUserService
    {
        public Task<bool> Exists(uint groupId, uint senderId);
        public Task<bool> Add(uint groupId, uint userId);
    }
}
