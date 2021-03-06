using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.ChatgroupService
{
    public abstract class ChatGroupServiceTestBase
    {
        protected readonly IChatGroupService sut;

        protected readonly Mock<IRepository<ChatGroup>> groupRepositoryMock;
        protected readonly Mock<IChatGroupUserService> groupUserServiceMock;
        protected readonly Mock<IHashAndSaltService> hashAndSaltServiceMock;
        protected readonly Mock<IUserService> userServiceMock;

        public ChatGroupServiceTestBase()
        {
            groupRepositoryMock = new();
            groupUserServiceMock = new();
            hashAndSaltServiceMock = new();
            userServiceMock = new();

            sut = new ChatCore.Services.ChatGroupService(groupRepositoryMock.Object, hashAndSaltServiceMock.Object, userServiceMock.Object);
        }
    }
}
