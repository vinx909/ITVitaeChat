using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Enums;

namespace ITVitaeChat.ChatCore.Entities
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxUsers { get; set; }
        public ChatGroupVisibility Visibility { get; set; }
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public int ModeratorId { get; set; }
        public User Moderator { get; set; }
        public bool OneToOne { get; set; }
    }
}
