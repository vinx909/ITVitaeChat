using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatMessageService
{
    public class Get : ChatMessageServiceTestBase
    {
        private const int amount = 10;

        [Fact]
        public void GetMessages()
        {
            //arrange
            const int chatgroup = 1;
            const int page = 0;
            IEnumerable<ChatMessage> messages = CreateMessages(amount, chatgroup);
            messageRepositoryMock.Setup(m => m.GetPage(chatgroup, amount, page)).Returns(Task.FromResult(messages));

            //act
            IEnumerable<ChatMessage> result = sut.Get(chatgroup, page).Result;

            //assert
            Assert.Equal(messages, result);
        }

        [Fact]
        public void GetSearchMessages()
        {
            //arrange
            const int chatgroup = 1;
            const int page = 0;
            const string searchTerm = "test";
            IEnumerable<ChatMessage> messages = CreateMessages(amount, chatgroup);
            messageRepositoryMock.Setup(m => m.SearchPage(It.IsAny<System.Linq.Expressions.Expression<Func<ChatMessage, bool>>>(), amount, page)).Returns(Task.FromResult(messages));

            //act
            IEnumerable<ChatMessage> result = sut.Get(chatgroup, searchTerm, page).Result;

            //assert
            Assert.Equal(messages, result);
        }

        private IEnumerable<ChatMessage> CreateMessages(int amount, uint chatgroup)
        {
            List<ChatMessage> messages = new();
            for (int i = 0; i < amount; i++)
            {
                messages.Add(new() { ChatgroupId = chatgroup, UserId = (uint)i % 3, SendTime = DateTime.Now.AddMinutes(-i), Content = "test message nr" + i, Id = (uint)i });
            }
            return messages;
        }
    }
}
