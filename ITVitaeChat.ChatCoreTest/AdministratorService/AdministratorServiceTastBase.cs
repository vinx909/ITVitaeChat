using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.AdministratorService
{
    public abstract class AdministratorServiceTastBase
    {
        protected readonly IAdministratorService sut;
        protected readonly Mock<IRepository<Administrator>> administratorRepositoryMock;

        public AdministratorServiceTastBase()
        {
            administratorRepositoryMock = new();
            sut = new ChatCore.Services.AdministratorService(administratorRepositoryMock.Object);
        }
    }
}
