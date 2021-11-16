using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private const int amountPerPage = 10;

        private IChatMessageRepository messageRepository;
        private IChatDisallowedWordsService disallowedWordsService;
        private IChatgroupUserService chatgroupUserService;

        public ChatMessageService(IChatMessageRepository messageRepository, IChatDisallowedWordsService disallowedWordsService, IChatgroupUserService chatgroupUserService)
        {
            this.messageRepository = messageRepository;
            this.disallowedWordsService = disallowedWordsService;
            this.chatgroupUserService = chatgroupUserService;
        }

        public async Task<bool> Add(string content, uint senderId, uint groupId)
        {
            if (await chatgroupUserService.Exists(groupId, senderId))
            {
                ChatMessage message = new() { ChatgroupId = groupId, UserId = senderId, SendTime = DateTime.Now };
                if(await disallowedWordsService.ContainsDisallowedWord(content))
                {
                    message.Content = await disallowedWordsService.FilterDisallowedWords(content);
                }
                else
                {
                    message.Content = content;
                }
                await messageRepository.Add(message);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<ChatMessage>> Get(uint groupId, int page)
        {
            return await messageRepository.GetPage(groupId, amountPerPage, page);
        }

        public async Task<IEnumerable<ChatMessage>> Get(int chatgroup, string searchTerm, int page)
        {
            return await messageRepository.SearchPage(m => m.ChatgroupId == chatgroup && m.Content == searchTerm, amountPerPage, page);
        }
    }
}