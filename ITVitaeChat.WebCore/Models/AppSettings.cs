using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.WebCore.Models
{
    public class AppSettings
    {
        public string SigninKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
