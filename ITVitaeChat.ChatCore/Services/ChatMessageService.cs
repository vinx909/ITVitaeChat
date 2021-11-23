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

        private readonly IChatMessageRepository messageRepository;
        private readonly IChatDisallowedWordsService disallowedWordsService;
        private readonly IChatGroupUserService chatgroupUserService;

        public ChatMessageService(IChatMessageRepository messageRepository, IChatDisallowedWordsService disallowedWordsService, IChatGroupUserService chatgroupUserService)
        {
            this.messageRepository = messageRepository;
            this.disallowedWordsService = disallowedWordsService;
            this.chatgroupUserService = chatgroupUserService;
        }

        public async Task<bool> Add(string content, int senderId, int groupId)
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

        public async Task<IEnumerable<ChatMessage>> Get(int groupId, int page)
        {
            return await messageRepository.GetPage(groupId, amountPerPage, page);
        }

        public async Task<IEnumerable<ChatMessage>> Get(int chatgroup, string searchTerm, int page)
        {
            return await messageRepository.SearchPage(m => m.ChatgroupId == chatgroup && m.Content == searchTerm, amountPerPage, page);
        }
    }
}