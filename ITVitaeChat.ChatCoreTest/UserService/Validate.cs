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
    public class Validate : UserServiceTestBase
    {
        [Fact]
        public void ValidatesUser()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            sut.Validate(1).Wait();

            //assert
            Assert.True(user.Validated);
        }
        [Fact]
        public void ValidateUserEdits()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            sut.Validate(1).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Edit(user), Times.Once);
        }
        [Fact]
        public void ValidateUserReturnsTrueOnSuccess()
        {
            //arrange
            const int id = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(user));

            //act
            bool result = sut.Validate(1).Result;

            //assert
            Assert.True(result);
        }
        [Fact]
        public void ValidateUserReturnsFalseOnUserNotFound()
        {
            //arrange
            const int id = 1;
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<User>(null));

            //act
            bool result = sut.Validate(1).Result;

            //assert
            Assert.False(result);
        }
    }
}
