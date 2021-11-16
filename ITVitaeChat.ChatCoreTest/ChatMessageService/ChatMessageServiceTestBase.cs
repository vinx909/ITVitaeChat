using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.ChatMessageService
{
    public abstract class ChatMessageServiceTestBase
    {
        protected readonly IChatMessageService sut;
        protected readonly Mock<IChatMessageRepository> messageRepositoryMock;
        protected readonly Mock<IChatDisallowedWordsService> disallowedWordsServiceMock;
        protected readonly Mock<IChatgroupUserService> groupUserServiceMock;

        public ChatMessageServiceTestBase()
        {
            messageRepositoryMock = new();
            disallowedWordsServiceMock = new();
            groupUserServiceMock = new();

            sut = new ChatCore.Services.ChatMessageService(messageRepositoryMock.Object, disallowedWordsServiceMock.Object, groupUserServiceMock.Object);
        }
    }
}
