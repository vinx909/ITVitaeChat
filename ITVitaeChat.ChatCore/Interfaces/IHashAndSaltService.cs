using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IHashAndSaltService
    {
        public bool Compare(string unhashed, string hashed, string salt);
        public string GenerateSalt();
        public string GenerateSalt(int saltLength);
        public string Hash(string value, string salt);
        public string Hash(string value, string salt, int numberOfIterations, int hashLength);
    }
}
