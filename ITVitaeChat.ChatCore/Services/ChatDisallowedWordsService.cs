using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class ChatDisallowedWordsService : IChatDisallowedWordsService
    {
        private const char maskingchar = '.';

        private readonly IRepository<ChatDisallowedWord> wordsRepository;

        public ChatDisallowedWordsService(IRepository<ChatDisallowedWord> disallowedWordsRepository)
        {
            this.wordsRepository = disallowedWordsRepository;
        }

        public async Task<bool> Add(string toAdd)
        {
            toAdd = toAdd.ToLower();
            if (await wordsRepository.Contains(w => w.Content == toAdd))
            {
                return false;
            }
            else
            {
                await wordsRepository.Add(new() { Content = toAdd });
                return true;
            }
        }

        public async Task<bool> ContainsDisallowedWord(string message)
        {
            message = message.ToLower();
            return await wordsRepository.Contains(w => message.Contains(w.Content));
        }

        public async Task<string> FilterDisallowedWords(string message)
        {
            foreach (ChatDisallowedWord word in await wordsRepository.GetAll())
            {
                if (message.ToLower().Contains(word.Content))
                {
                    string replacement = ""+maskingchar;
                    for (int i = 1; i < word.Content.Length; i++)
                    {
                        replacement += maskingchar;
                    }
                    message = message.Replace(word.Content, replacement, StringComparison.OrdinalIgnoreCase);
                }
            }
            return message;
        }

        public async Task<bool> Remove(string toRemove)
        {
            toRemove = toRemove.ToLower();
            ChatDisallowedWord word = await wordsRepository.Get(w => w.Content == toRemove);
            if(word == null)
            {
                return false;
            }
            else
            {
                await wordsRepository.Delete(word);
                return true;
            }
        }
    }
}
