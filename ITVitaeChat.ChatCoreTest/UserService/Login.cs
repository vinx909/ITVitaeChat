using System.Collections.Generic;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.UserService
{
    public class Login : UserServiceTestBase
    {
        [Fact]
        public void CorrectLoginStatusLoggedIn()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(LoginResult.loggedIn, result.Item1);
        }
        [Fact]
        public void CorrectLoginReturnsUser()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(user, result.Item2);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("abc", null)]
        [InlineData("abc", "")]
        [InlineData("abc", " ")]
        [InlineData("abc", "   ")]
        [InlineData(null, "jkl")]
        [InlineData("", "jkl")]
        [InlineData(" ", "jkl")]
        [InlineData("   ", "jkl")]
        public void IncompleteFilledInLoginStatusIncompleteData(string username, string password)
        {
            //arrange
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            User user = new() { DisplayName = username, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == username)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(username, password).Result;

            //assert
            Assert.Equal(LoginResult.incompleteData, result.Item1);
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData("abc", null)]
        [InlineData("abc", "")]
        [InlineData("abc", " ")]
        [InlineData("abc", "   ")]
        [InlineData(null, "jkl")]
        [InlineData("", "jkl")]
        [InlineData(" ", "jkl")]
        [InlineData("   ", "jkl")]
        public void IncompleteFilledInLoginReturnsNull(string username, string password)
        {
            //arrange
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            User user = new() { DisplayName = username, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == username)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(username, password).Result;

            //assert
            Assert.Null(result.Item2);
        }

        [Fact]
        public void DisplayNameNotPresentStatusUsernameNotFound()
        {
            //arrange
            const string userDisplayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            const string loginDisplayName = "mno";
            User user = new() { DisplayName = userDisplayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == loginDisplayName)).Returns(Task.FromResult<User>(null));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(loginDisplayName, password).Result;

            //assert
            Assert.Equal(LoginResult.usernameNotFound, result.Item1);
        }
        [Fact]
        public void DisplayNameNotPresentReturnsNull()
        {
            //arrange
            const string userDisplayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            const string loginDisplayName = "mno";
            User user = new() { DisplayName = userDisplayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == loginDisplayName)).Returns(Task.FromResult<User>(null));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(loginDisplayName, password).Result;

            //assert
            Assert.Null(result.Item2);
        }
        [Fact]
        public void IncorrectPasswordStatusIncorrectPassword()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(false);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(LoginResult.incorrectPassword, result.Item1);
        }
        [Fact]
        public void IncorrectPasswordReturnsNull()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(false);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Null(result.Item2);
        }

        [Fact]
        public void BlockedUserStatusBlocked()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true, Blocked = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(LoginResult.blocked, result.Item1);
        }
        [Fact]
        public void BlockedUserReturnsUser()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = true, Blocked = true };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(user, result.Item2);
        }

        [Fact]
        public void UnvalidatedUserStatusNotValidated()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = false};
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(LoginResult.notValidated, result.Item1);
        }
        [Fact]
        public void UnvalidatedUserStatusReturnsUser()
        {
            //arrange
            const string displayName = "abc";
            const string hashedPassword = "def";
            const string passwordSalt = "ghi";
            const string password = "jkl";
            User user = new() { DisplayName = displayName, Password = hashedPassword, PasswordSalt = passwordSalt, Validated = false };
            userRepositoryMock.Setup(m => m.Get(u => u.DisplayName == displayName)).Returns(Task.FromResult(user));
            hashAndSaltMock.Setup(m => m.Compare(password, hashedPassword, passwordSalt)).Returns(true);

            //act
            (LoginResult, User) result = sut.Login(displayName, password).Result;

            //assert
            Assert.Equal(user, result.Item2);
        }
    }
}
