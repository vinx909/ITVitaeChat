using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.ChatCore.Entities
{
    public class ChatMessage
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public uint ChatgroupId { get; set; }
        public DateTime SendTime { get; set; }
        public string Content { get; set; }

        public User User { get; set; }
        public Chatgroup Chatgroup { get; set; }
    }
}
