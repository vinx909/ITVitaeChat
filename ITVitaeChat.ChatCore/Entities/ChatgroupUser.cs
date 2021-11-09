using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.ChatCore.Entities
{
    public class ChatgroupUser
    {
        public uint UserId { get; set; }
        public uint ChatgroupId { get; set; }
        public bool OneToOne { get; set; }
        public User User { get; set; }
        public Chatgroup Chatgroup { get; set; }
    }
}
