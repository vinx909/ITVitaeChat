using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.UserService
{
    public class Block : UserServiceTestBase
    {
        [Fact]
        public void BlockUserBlocks()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            sut.Block(1).Wait();

            //assert
            Assert.True(user.Blocked);
        }
        [Fact]
        public void BlockUserUnblocks()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = true, Blocked = true };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            sut.Block(1).Wait();

            //assert
            Assert.False(user.Blocked);
        }
        [Fact]
        public void BlockUserEdits()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            sut.Block(1).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(user), Times.Once);
        }
        [Fact]
        public void BlockUserReturnsTrueOnSuccess()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            bool result = sut.Block(1).Result;

            //assert
            Assert.True(result);
        }
        [Fact]
        public void BlockUserReturnsFalseOnUserNotFound()
        {
            //arrange
            const int id = 1;
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<User>(null));

            //act
            bool result = sut.Block(1).Result;

            //assert
            Assert.False(result);
        }
    }
}
