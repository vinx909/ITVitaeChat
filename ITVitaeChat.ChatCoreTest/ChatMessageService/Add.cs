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
    public class Add : ChatMessageServiceTestBase
    {
        [Fact]
        public void ValuesCorrectAdds()
        {
            //arrange
            const string content = "test message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(true));
            

            //act
            sut.Add(content, senderId, groupId).Wait();

            //assert
            messageRepositoryMock.Verify(m => m.Add(It.Is<ChatMessage>(m => m.Content.Equals(content) && m.UserId == senderId && m.ChatgroupId == groupId)), Times.Once);
        }

        [Fact]
        public void ValuesCorrectReturnsTrue()
        {
            //arrange
            const string content = "test message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(content, senderId, groupId).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ValuesCorrectAddsWithCurrentDateTime()
        {
            //arrange
            const string content = "test message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(true));

            //act
            DateTime startTime = DateTime.Now;
            sut.Add(content, senderId, groupId).Wait();
            DateTime endTime = DateTime.Now;

            //assert
            messageRepositoryMock.Verify(m => m.Add(It.Is<ChatMessage>(m => m.SendTime>=startTime && m.SendTime<=endTime)), Times.Once);
        }
        
        [Fact]
        public void ValuesGroupUserDoesNotExistDoesNotAdd()
        {
            //arrange
            const string content = "test message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(false));

            //act
            sut.Add(content, senderId, groupId).Wait();

            //assert
            messageRepositoryMock.Verify(m => m.Add(It.IsAny<ChatMessage>()), Times.Never);
        }

        [Fact]
        public void ValuesGroupUserDoesNotExistReturnsFalse()
        {
            //arrange
            const string content = "test message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(false));

            //act
            bool result = sut.Add(content, senderId, groupId).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ValuesMessagedContainsDisallowedWordsChangeMessage()
        {
            //arrange
            const string content = "test message";
            const string cencorredMessage = ".... message";
            const uint senderId = 1;
            const uint groupId = 1;
            groupUserServiceMock.Setup(m => m.Exists(groupId, senderId)).Returns(Task.FromResult(true));
            disallowedWordsServiceMock.Setup(m => m.ContainsDisallowedWord(content)).Returns(Task.FromResult(true));
            disallowedWordsServiceMock.Setup(m => m.FilterDisallowedWords(content)).Returns(Task.FromResult(cencorredMessage));

            //act
            sut.Add(content, senderId, groupId);

            //assert
            messageRepositoryMock.Verify(m => m.Add(It.Is<ChatMessage>(c => c.Content.Equals(cencorredMessage))), Times.Once);
        }
    }
}
