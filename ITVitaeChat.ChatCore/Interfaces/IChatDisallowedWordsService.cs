using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IChatDisallowedWordsService
    {
        public Task<bool> Add(string toAdd);
        public Task<bool> Remove(string toRemove);
        public Task<bool> ContainsDisallowedWord(string message);
        public Task<string> FilterDisallowedWords(string message);
    }
}
