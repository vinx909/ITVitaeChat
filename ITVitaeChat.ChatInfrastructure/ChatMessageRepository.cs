using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITVitaeChat.ChatInfrastructure
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(ITVitaeChatDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<ChatMessage>> GetPage(int chatGroup, int amountPerPage, int page)
        {
            return await dbContext.Set<ChatMessage>().Where(m => m.ChatgroupId == chatGroup).OrderByDescending(m => m.SendTime).Skip(amountPerPage * page).Take(amountPerPage).ToArrayAsync();
        }

        public async Task<IEnumerable<ChatMessage>> SearchPage(Expression<Func<ChatMessage, bool>> query, int amountPerPage, int page)
        { 
            return await dbContext.Set<ChatMessage>().Where(query).OrderByDescending(m => m.SendTime).Skip(amountPerPage * page).Take(amountPerPage).ToArrayAsync();
        }
    }
}
