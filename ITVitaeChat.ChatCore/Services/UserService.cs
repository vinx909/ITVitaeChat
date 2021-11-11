using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class UserService : IUserService
    {
        private const int nameMaxLength = 255;
        private const int displayNameMaxLength = 255;
        private const int emailadresMaxLength = 255;
        private const int passwordMaxLength = 255;

        private readonly IRepository<User> userRepository;
        private readonly IHashAndSaltService hashAndSalt;

        public UserService(IRepository<User> userRepository, IHashAndSaltService hashAndSalt)
        {
            this.userRepository = userRepository;
            this.hashAndSalt = hashAndSalt;
        }

        public async Task<bool> Register(User user)
        {
            if (UserFilledIn(user) && ValidateUserValues(user.Name, user.DisplayName, user.Emailadres, user.Password) && ValidateEmailadres(user.Emailadres) && !userRepository.Contains(u => u.DisplayName.Equals(user.DisplayName) || u.Emailadres.Equals(user.Emailadres)).Result)
            {
                user.PasswordSalt = hashAndSalt.GenerateSalt();
                user.Password = hashAndSalt.Hash(user.Password, user.PasswordSalt);
                user.Blocked = false;
                user.Validated = false;
                await userRepository.Add(user);
                return true;
            }
            return false;
        }
        public async Task<(LoginResult, User)> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new(LoginResult.incompleteData, null);
            }
            else
            {
                User user = await userRepository.Get(u => u.DisplayName == username);
                if(user == null)
                {
                    return new(LoginResult.usernameNotFound, null);
                }
                else if (hashAndSalt.Compare(password, user.Password, user.PasswordSalt))
                {
                    if (user.Validated == false)
                    {
                        return new(LoginResult.notValidated, user);
                    }
                    else if (user.Blocked == true)
                    {
                        return new(LoginResult.blocked, user);
                    }
                    else
                    {
                        return new(LoginResult.loggedIn, user);
                    }
                }
                else
                {
                    return new(LoginResult.incorrectPassword, null);
                }
            }
        }
        public Task<bool> Edit(uint id, string username, string password, string emailadress)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Edit(User user)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Validate(uint id)
        {
            User user = await userRepository.Get(id);
            if (user != null)
            {
                user.Validated = true;
                await userRepository.Edit(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> Block(uint id)
        {
            User user = await userRepository.Get(id);
            if(user != null)
            {
                user.Blocked = !user.Blocked;
                await userRepository.Edit(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool UserFilledIn(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.DisplayName) && !string.IsNullOrWhiteSpace(user.Emailadres) && !string.IsNullOrWhiteSpace(user.Password);
        }
        private static bool ValidateUserValues(string name, string displayName, string emailadres, string password)
        {
            return name.Length <= nameMaxLength && ValidateUserValues(displayName, emailadres, password);
        }
        private static bool ValidateUserValues(string displayName, string emailadres, string password)
        {
            return displayName.Length <= displayNameMaxLength && emailadres.Length <= emailadresMaxLength && password.Length <= passwordMaxLength;
        }
        private static bool ValidateEmailadres(string emailadres)
        {
            //Todo email validationcheck
            return true;
        }

    }
}
