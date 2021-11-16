﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        public Task<IEnumerable<ChatMessage>> GetPage(uint chatGroup, int amountPerPage, int page);
        public Task<IEnumerable<ChatMessage>> SearchPage(Expression<Func<ChatMessage, bool>> expression, int amountPerPage, int page);
    }
}
