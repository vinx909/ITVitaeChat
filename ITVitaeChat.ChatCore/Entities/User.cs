using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.ChatCore.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int IdentityId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Emailadres { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool Validated { get; set; }
        public bool Blocked { get; set; }
    }
}
