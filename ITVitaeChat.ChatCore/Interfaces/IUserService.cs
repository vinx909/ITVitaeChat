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
        public Task<bool> Register(string name, string displayName, string emailadres, string password, IChatGroupUserService groupUserService = null);
        public Task<bool> Register(User user, IChatGroupUserService groupUserService = null);
        public Task<bool> Edit(int id, string displayName, string password, string emailadress);
        public Task<bool> Edit(User user);
        public Task<(LoginResult, User)> Login(string username, string password);
        public Task<bool> Exists(int userId);
        public Task<bool> Block(int id);
        public Task<bool> Validate(int id);
    }
}
