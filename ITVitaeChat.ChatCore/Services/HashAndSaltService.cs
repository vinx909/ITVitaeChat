using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class HashAndSaltService : IHashAndSaltService
    {
        private const int saltLength = 16;
        private const int hashLength = 20;
        private const int nItterations = 10000;

        public bool Compare(string unhashed, string hashed, string salt)
        {
            return hashed.Equals(Hash(unhashed, salt));
        }

        public string GenerateSalt()
        {
            return GenerateSalt(saltLength);
        }
        public string GenerateSalt(int saltLength)
        {
            var saltBytes = new byte[saltLength];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public string Hash(string value, string salt)
        {
            return Hash(value, salt, nItterations, hashLength + saltLength);
        }
        public string Hash(string value, string salt, int numberOfIterations, int hashLength)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(value, saltBytes, numberOfIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(hashLength));
            }
        }
    }
}
