using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.UserService
{
    public class Exists : UserServiceTestBase
    {
        [Fact]
        public void IfExistsReturnsTrue()
        {
            //arrange
            const uint id = 1;
            userRepositoryMock.Setup(m => m.Contains(id)).Returns(Task.FromResult(true));

            //act
            bool result = sut.Exists(id).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfDoesNotExistsReturnsFalse()
        {
            //arrange
            const uint id = 1;
            userRepositoryMock.Setup(m => m.Contains(id)).Returns(Task.FromResult(false));

            //act
            bool result = sut.Exists(id).Result;

            //assert
            Assert.False(result);
        }
    }
}
