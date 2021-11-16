using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.ChatgroupUserService
{
    public abstract class ChatgroupUserServiceTestBase
    {
        protected readonly IChatgroupUserService sut;
        protected readonly Mock<IRepository<ChatgroupUser>> groupUserRepositoryMock;
        protected readonly Mock<IChatGroupService> groupServiceMock;
        protected readonly Mock<IUserService> userServiceMock;

        public ChatgroupUserServiceTestBase()
        {
            groupUserRepositoryMock = new();
            groupServiceMock = new();
            userServiceMock = new();

            sut = new ChatCore.Services.ChatGroupUserService(groupUserRepositoryMock.Object, groupServiceMock.Object, userServiceMock.Object);
        }
    }
}
