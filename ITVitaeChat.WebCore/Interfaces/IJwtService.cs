using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.WebCore.Interfaces
{
    public interface IJwtService
    {
        public string Generate(List<Claim> claims);
    }
}
