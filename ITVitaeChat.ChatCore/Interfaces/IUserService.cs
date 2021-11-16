using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IUserService
    {
        public Task<bool> Register(User user);
        public Task<bool> Edit(uint id, string displayName, string password, string emailadress);
        public Task<bool> Edit(User user);
        public Task<(LoginResult, User)> Login(string username, string password);
        public Task<bool> Exists(uint userId);
        public Task<bool> Block(uint id);
        public Task<bool> Validate(uint id);
    }
}
