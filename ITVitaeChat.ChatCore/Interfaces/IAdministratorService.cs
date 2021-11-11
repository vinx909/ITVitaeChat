using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IAdministratorService
    {
        public Task<bool> Add(Administrator administrator);
        public Task<Administrator> Get(uint Id);
    }
}
