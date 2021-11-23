using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class UserService : IUserService
    {
        private const int generalGroupId = 1;

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

        public async Task<bool> Register(string name, string displayName, string emailadres, string password, IChatGroupUserService groupUserService = null)
        {
            return await Register(new() { Name = name, DisplayName = displayName, Emailadres = emailadres, Password = password}, groupUserService);
        }
        public async Task<bool> Register(User user, IChatGroupUserService groupUserService = null)
        {
            if (ValidateName(user.Name) && ValidateDisplayName(user.DisplayName) && ValidatePassword(user.Password) && ValidateEmailadres(user.Emailadres) && !userRepository.Contains(u => u.DisplayName.Equals(user.DisplayName) || u.Emailadres.Equals(user.Emailadres)).Result)
            {
                user.PasswordSalt = hashAndSalt.GenerateSalt();
                user.Password = hashAndSalt.Hash(user.Password, user.PasswordSalt);
                user.Blocked = false;
                user.Validated = false;
                int? id = await userRepository.Add(user);
                if(groupUserService != null && id != null)
                {
                    await groupUserService.Add(generalGroupId, (int)id);
                }
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
        public async Task<bool> Edit(int id, string displayName, string password, string emailadress)
        {
            //check if displayname, password and emailadress are valid and not all three null. if not return false
            if (!ValidateDisplayName(displayName, true) || !ValidatePassword(password, true) || !ValidateEmailadres(emailadress, true) || (displayName==null && password == null && emailadress == null))
            {
                return false;
            }

            //try to get the user from the repository, returning false if it can't be found
            User user = await userRepository.Get(id);
            if (user != null)
            {
                //checks if the repository contains a user that doesn't match the id but does match the displayname or emailadress, so the same user can keep the same values, but it can't have the values of another user. if such value already exists returns false
                if (userRepository.Contains(u => u.Id.Equals(id) && (u.DisplayName.Equals(displayName)||u.Emailadres.Equals(emailadress))).Result)
                {
                    return false;
                }
                if (displayName != null)
                    user.DisplayName = displayName;
                if (emailadress != null)
                    user.Emailadres = emailadress;
                if (password != null)
                    user.Password = hashAndSalt.Hash(password, user.PasswordSalt);
                await userRepository.Update(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> Edit(User user)
        {
            return await Edit(user.Id, user.DisplayName, user.Password, user.Emailadres);
        }
        public async Task<bool> Validate(int id)
        {
            User user = await userRepository.Get(id);
            if (user != null)
            {
                user.Validated = true;
                await userRepository.Update(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> Block(int id)
        {
            User user = await userRepository.Get(id);
            if (user != null)
            {
                user.Blocked = !user.Blocked;
                await userRepository.Update(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateName(string name, bool allowNull = false)
        {
            if (name == null && allowNull == true)
            {
                return true;
            }
            else
            {
                return !string.IsNullOrWhiteSpace(name) && name.Length <= nameMaxLength;
            }
        }
        private static bool ValidateDisplayName(string displayName, bool allowNull = false)
        {
            if (displayName == null && allowNull == true)
            {
                return true;
            }
            else
            {
                return !string.IsNullOrWhiteSpace(displayName) && displayName.Length <= displayNameMaxLength;
            }
        }
        private static bool ValidateEmailadres(string emailadres, bool allowNull = false)
        {
            if (emailadres == null && allowNull == true)
            {
                return true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(emailadres) || emailadres.Length > emailadresMaxLength)
                {
                    return false;
                }
                if (!MailAddress.TryCreate(emailadres, out MailAddress mailAddress))
                {
                    return false;
                }

                var hostParts = mailAddress.Host.Split('.');
                if (hostParts.Length == 1)// checks if the host is missing a dot and is thus not valid
                {
                    return false;
                }
                if (hostParts.Any(p => p == string.Empty))//checks if there's any empty strings between dots, which thus tests for if there are two donts next to each other. eg: something@somethi..ng
                {
                    return false;
                }
                if (hostParts[^1].Length < 2 || hostParts[^1].Length > 3) //checks if the last element has not less then 2 and not more then 3 elements
                {
                    return false;
                }

                return true;
            }
        }
        private static bool ValidatePassword(string password, bool allowNull = false)
        {
            if (password == null && allowNull == true)
            {
                return true;
            }
            else
            {
                return !string.IsNullOrWhiteSpace(password) && password.Length <= passwordMaxLength;
            }
        }

        public async Task<bool> Exists(int senderId)
        {
            return await userRepository.Contains(senderId);
        }

    }
}
