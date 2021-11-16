using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatgroupService
{
    public class Create : ChatGroupServiceTestBase
    {
        [Fact]
        public void CorrectAddsGroup()
        {
            //arrange
            uint? id = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(id));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt.Equals(passwordSalt) && g.Password.Equals(passwordHashed) && g.ModeratorId.Equals(userId))), Times.Once);
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<Chatgroup>()), Times.Once);
        }
        [Fact]
        public void CorrectAddsModeratorToGroupUser()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupUserServiceMock.Verify(m => m.Add((uint)groupId, userId));
        }

        [Fact]
        public void CorrectAddsGroupDefaultMaxUsers()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt.Equals(passwordSalt) && g.Password.Equals(passwordHashed) && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void CorrectAddsGroupDefaultvisibility()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const uint userId = 1;
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Create(name, maxUsers, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<Chatgroup>()), Times.Once);
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt == null && g.Password == null && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void CorrectAddsGroupDefaultVisibilityAndMaxUsers()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const uint userId = 1;
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Create(name, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }

        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void UserDoesNotExistDoesNotAdd()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<Chatgroup>()), Times.Never);
        }
        [Fact]
        public void UserDoesNotExistReturnsFalse()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void VisibilityPublicPassesPasswordAsNull()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const string password = "def";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }

        [Fact]
        public void VisibilityPublicPasswordNullPasses()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));

            //act
            sut.Create(name, maxUsers, visibility, null, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<Chatgroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void VisibilityPublicPasswordNullReturnsTrue()
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Public;
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));

            //act
            bool result = sut.Create(name, maxUsers, visibility, null, userId).Result;

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void NameEmptyOrNullDoesNotPass(string name)
        {
            //arrange
            uint? groupId = 1;
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<Chatgroup>()), Times.Never);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void NameEmptyOrNullReturnsFalse(string name)
        {
            //arrange
            uint? groupId = 1;
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId).Result;

            //assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void VisibilityPrivatePasswordEmptyOrNullDoesNotPass(string password)
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<Chatgroup>()), Times.Never);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void VisibilityPrivatePasswordEmptyOrNullReturnsFalse(string password)
        {
            //arrange
            uint? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatgroupVisibility visibility = ChatgroupVisibility.Private;
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const uint userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<Chatgroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId).Result;

            //assert
            Assert.False(result);
        }
    }
}
