using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCoreTest.HashAndSaltService
{
    public abstract class HashAndSaltServiceTestBase
    {
        protected IHashAndSaltService sut;

        public HashAndSaltServiceTestBase()
        {
            sut = new ChatCore.Services.HashAndSaltService();
        }
    }
}
