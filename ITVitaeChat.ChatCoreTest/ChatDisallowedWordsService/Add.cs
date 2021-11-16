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
    public class Add : ChatDisallowedWordsServiceTestBase
    {
        [Fact]
        public void WhenCorrectAdds()
        {
            //arrange
            const string toAdd = "a";

            //act
            sut.Add(toAdd).Wait();

            //assert
            disallowedWordsRepositoryMock.Verify(m => m.Add(It.Is<ChatDisallowedWord>(w => w.Content==toAdd)), Times.Once);
        }

        [Fact]
        public void WhenCorrectReturnsTrue()
        {
            //arrange
            const string toAdd = "a";

            //act
            bool result = sut.Add(toAdd).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfAlreadyExistsDoesNotAdd()
        {
            //arrange
            const string toAdd = "a";
            disallowedWordsRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Add(toAdd).Wait();

            //assert
            disallowedWordsRepositoryMock.Verify(m => m.Add(It.Is<ChatDisallowedWord>(w => w.Content == toAdd)), Times.Never);
        }

        [Fact]
        public void IfAlreadyExistsReturnsFalse()
        {
            //arrange
            const string toAdd = "a";
            disallowedWordsRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatDisallowedWord, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(toAdd).Result;

            //assert
            Assert.False(result);
        }
    }
}
