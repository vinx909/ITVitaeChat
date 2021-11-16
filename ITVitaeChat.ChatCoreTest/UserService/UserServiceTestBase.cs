using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.UserService
{
    public abstract class UserServiceTestBase
    {
        protected readonly IUserService sut;
        protected readonly Mock<IRepository<User>> userRepositoryMock;
        protected readonly Mock<IHashAndSaltService> hashAndSaltMock;

        public UserServiceTestBase()
        {
            userRepositoryMock = new();
            hashAndSaltMock = new();
            sut = new ChatCore.Services.UserService(userRepositoryMock.Object, hashAndSaltMock.Object);
        }
    }
}
