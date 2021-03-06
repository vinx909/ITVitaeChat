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
    public class Register : UserServiceTestBase
    {
        [Fact]
        public void UserAddWithFullUser()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };

            //act
            sut.Register(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(user), Times.Once);
        }
        [Fact]
        public void ValuesAddWithFullUser()
        {
            //arrange
            const string name = "abc";
            const string displayName = "def";
            const string emailadres = "ghi@jkl.mno";
            const string password = "pqr";

            //act
            sut.Register(name, displayName, emailadres, password).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(It.Is<User>(u => u.Name == name && u.DisplayName == displayName && u.Emailadres == emailadres)), Times.Once);
        }

        [Fact]
        public void UserReturnsTrueWithFullUser()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };

            //act
            bool result = sut.Register(user).Result;

            //assert
            Assert.True(result);
        }
        [Fact]
        public void ValuesReturnsTrueWithFullUser()
        {
            //arrange
            const string name = "abc";
            const string displayName = "def";
            const string emailadres = "ghi@jkl.mno";
            const string password = "pqr";

            //act
            bool result = sut.Register(name, displayName, emailadres, password).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void AddWithFullUserAddsToGeneralChat()
        {
            //arrange
            int? userId = 3;
            const int generalGroupId = 1;
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Add(user)).Returns(Task.FromResult(userId));

            //act
            sut.Register(user, groupUserServiceMock.Object).Wait();

            //assert
            groupUserServiceMock.Verify(m => m.Add(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            groupUserServiceMock.Verify(m => m.Add(generalGroupId, (int)userId), Times.Once);
        }

        [Theory]
        [InlineData(null, "def", "ghi@jkl.mno", "pqr")]
        [InlineData("", "def", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "def", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "def", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", null, "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", " ", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "   ", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "def", null, "pqr")]
        [InlineData("abc", "def", "", "pqr")]
        [InlineData("abc", "def", " ", "pqr")]
        [InlineData("abc", "def", "   ", "pqr")]
        [InlineData("abc", "def", "ghi@jkl.mno", null)]
        [InlineData("abc", "def", "ghi@jkl.mno", "")]
        [InlineData("abc", "def", "ghi@jkl.mno", " ")]
        [InlineData("abc", "def", "ghi@jkl.mno", "   ")]
        public void IncompleteUserDoesNotPass(string name, string displayName, string emailadres, string password)
        {
            //arrange
            User user = new() { Name = name, DisplayName = displayName, Emailadres = emailadres, Password = password };

            //act
            sut.Register(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(user), Times.Never);
        }
        [Theory]
        [InlineData(null, "def", "ghi@jkl.mno", "pqr")]
        [InlineData("", "def", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "def", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "def", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", null, "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", " ", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "   ", "ghi@jkl.mno", "pqr")]
        [InlineData("abc", "def", null, "pqr")]
        [InlineData("abc", "def", "", "pqr")]
        [InlineData("abc", "def", " ", "pqr")]
        [InlineData("abc", "def", "   ", "pqr")]
        [InlineData("abc", "def", "ghi@jkl.mno", null)]
        [InlineData("abc", "def", "ghi@jkl.mno", "")]
        [InlineData("abc", "def", "ghi@jkl.mno", " ")]
        [InlineData("abc", "def", "ghi@jkl.mno", "   ")]
        public void IncompleteUserReturnsFalse(string name, string displayName, string emailadres, string password)
        {
            //arrange
            User user = new() { Name = name, DisplayName = displayName, Emailadres = emailadres, Password = password };

            //act
            bool result = sut.Register(user).Result;

            //assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void IncorrectEmailadressDoesNotPass(string emailadres)
        {
            //arrange
            User user = new() { Name = "ghi", DisplayName = "jkl", Emailadres = emailadres, Password = "mno" };

            //act
            sut.Register(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(user), Times.Never);
        }
        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void IncorrectEmailadressReturnsFalse(string emailadres)
        {
            //arrange
            User user = new() { Name = "ghi", DisplayName = "jkl", Emailadres = emailadres, Password = "mno" };

            //act
            bool result = sut.Register(user).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void DoesNotPassWithAlreadyExistingValues()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Register(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(user), Times.Never);
        }
        [Fact]
        public void ReturnsFalseWithAlreadyExistingValues()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Register(user).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void AddsSalt()
        {
            //arange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            const string salt = "stu";
            hashAndSaltMock.Setup(m => m.GenerateSalt()).Returns(salt);

            //act
            sut.Register(user);

            //assert
            Assert.Equal(salt, user.PasswordSalt);
        }
        [Fact]
        public void HashesPasswordCallsInterface()
        {
            //arange
            const string password = "pqr";
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = password };
            const string salt = "stu";
            hashAndSaltMock.Setup(m => m.GenerateSalt()).Returns(salt);
            hashAndSaltMock.Setup(m => m.Hash(user.Password, salt));

            //act
            sut.Register(user);

            //assert
            hashAndSaltMock.Verify(m => m.Hash(password, salt), Times.Once);
        }
        [Fact]
        public void HashesPasswordAltersPassword()
        {
            //arange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            const string salt = "stu";
            const string hashedPassword = "vwx";
            hashAndSaltMock.Setup(m => m.GenerateSalt()).Returns(salt);
            hashAndSaltMock.Setup(m => m.Hash(user.Password, salt)).Returns(hashedPassword);

            //act
            sut.Register(user);

            //assert
            Assert.Equal(hashedPassword, user.Password);
        }

        [Fact]
        public void SetBockedFalse()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Blocked = true };

            //act
            sut.Register(user).Wait();

            //assert
            Assert.False(user.Blocked);
        }
        [Fact]
        public void SetsBockedFalse()
        {
            //arrange
            User user = new() { Name = "abc", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", Validated = true };

            //act
            sut.Register(user).Wait();

            //assert
            Assert.False(user.Validated);
        }
    }
}
