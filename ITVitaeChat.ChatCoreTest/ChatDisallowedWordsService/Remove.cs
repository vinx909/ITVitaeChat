using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatDisallowedWordsService
{
    public class Remove : ChatDisallowedWordsServiceTestBase
    {
        [Fact]
        public void WhenCorrectRemoves()
        {
            //arrange
            const string toRemove = "a";
            const uint id = 1;
            ChatDisallowedWord disallowedWord = new() { Id = id, Content = toRemove };
            disallowedWordsRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(disallowedWord));

            //act
            sut.Remove(toRemove).Wait();

            //assert
            disallowedWordsRepositoryMock.Verify(m => m.Delete(disallowedWord), Times.Once);
        }

        [Fact]
        public void WhenCorrectReturnsTrue()
        {
            //arrange
            const string toRemove = "a";
            const uint id = 1;
            ChatDisallowedWord disallowedWord = new() { Id = id, Content = toRemove };
            disallowedWordsRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(disallowedWord));

            //act
            bool result = sut.Remove(toRemove).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void WhenDoesNotExistDoesNotTryToRemove()
        {
            //arrange
            const string toRemove = "a";
            disallowedWordsRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult<ChatDisallowedWord>(null));

            //act
            sut.Remove(toRemove).Wait();

            //assert
            disallowedWordsRepositoryMock.Verify(m => m.Delete(It.IsAny<ChatDisallowedWord>()), Times.Never);
        }

        [Fact]
        public void WhenDoesNotExistReturnsFalse()
        {
            //arrange
            const string toRemove = "a";
            disallowedWordsRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult<ChatDisallowedWord>(null));

            //act
            bool result = sut.Remove(toRemove).Result;

            //assert
            Assert.False(result);
        }
    }
}
