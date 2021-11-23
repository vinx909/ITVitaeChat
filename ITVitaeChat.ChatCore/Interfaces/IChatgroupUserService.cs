using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatGroupUserService
    {
        public Task<bool> Exists(int groupId, int senderId);
        public Task<bool> Add(int groupId, int userId);
        public Task<bool> Remove(int groupid, int userId);
        public Task<IEnumerable<int>> GetGroupUsers(int groupId);
    }
}
