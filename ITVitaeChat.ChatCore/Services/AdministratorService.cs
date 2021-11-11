using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;

namespace ITVitaeChat.ChatCore.Services
{
    public class AdministratorService : IAdministratorService
    {
        private IRepository<Administrator> administratorRepository;

        public AdministratorService(IRepository<Administrator> administratorRepository)
        {
            this.administratorRepository = administratorRepository;
        }

        public async Task<bool> Add(Administrator administrator)
        {
            if(!await administratorRepository.Contains(a => a.UserId == administrator.UserId))
            {
                await administratorRepository.Add(administrator);
                return true;
            }
            return false;
        }

        public async Task<Administrator> Get(uint id)
        {
            return await administratorRepository.Get(id);
        }
    }
}
