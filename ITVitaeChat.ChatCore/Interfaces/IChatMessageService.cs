using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatMessageService
    {
        public Task<bool> Add(string content, uint senderId, uint groupId);
        public Task<IEnumerable<ChatMessage>> Get(uint groupId, int page);
        public Task<IEnumerable<ChatMessage>> Get(int chatgroup, string searchTerm, int page);
    }
}
