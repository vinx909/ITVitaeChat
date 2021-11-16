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
    public class ContainsDisallowedWord : ChatDisallowedWordsServiceTestBase 
    {
        //i find this one hard to test as i fail to correctly pass a lambda expression

        [Fact]
        public void IfContainsReturnsTrue()
        {
            //arrange
            const string message = "abcdefg";
            disallowedWordsRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.ContainsDisallowedWord(message).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfNotContainsReturnsFalse()
        {
            //arrange
            const string message = "abcdefg";
            disallowedWordsRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(false));

            //act
            bool result = sut.ContainsDisallowedWord(message).Result;

            //assert
            Assert.False(result);
        }
    }
}
