using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using Moq;

namespace ITVitaeChat.ChatCoreTest.ChatDisallowedWordsService
{
    public abstract class ChatDisallowedWordsServiceTestBase
    {
        protected readonly IChatDisallowedWordsService sut;
        protected readonly Mock<IRepository<ChatDisallowedWord>> disallowedWordsRepositoryMock;

        public ChatDisallowedWordsServiceTestBase()
        {
            disallowedWordsRepositoryMock = new();
            sut = new ChatCore.Services.ChatDisallowedWordsService(disallowedWordsRepositoryMock.Object);
        }
    }
}
