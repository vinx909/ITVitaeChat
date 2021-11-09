using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Enums;

namespace ITVitaeChat.ChatCore.Entities
{
    public class Chatgroup
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public int MaxUsers { get; set; }
        public ChatgroupVisibility Visibility { get; set; }
        public string Password { get; set; }
        public uint ModeratorId { get; set; }
        public User Moderator { get; set; }
    }
}
