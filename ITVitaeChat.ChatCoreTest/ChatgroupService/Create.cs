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
            int? id = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(id));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt.Equals(passwordSalt) && g.Password.Equals(passwordHashed) && g.ModeratorId.Equals(userId))), Times.Once);
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Once);
        }
        [Fact]
        public void CorrectAddsModeratorToGroupUser()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupUserServiceMock.Verify(m => m.Add((int)groupId, userId));
        }

        [Fact]
        public void CorrectChatGroupUserServiceNullAddsGroup()
        {
            //arrange
            int? id = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(id));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, null).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt.Equals(passwordSalt) && g.Password.Equals(passwordHashed) && g.ModeratorId.Equals(userId))), Times.Once);
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Once);
        }

        [Fact]
        public void CorrectAddsGroupDefaultMaxUsers()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt.Equals(passwordSalt) && g.Password.Equals(passwordHashed) && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void CorrectAddsGroupDefaultvisibility()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const int userId = 1;
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Create(name, maxUsers, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Once);
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt == null && g.Password == null && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void CorrectAddsGroupDefaultVisibilityAndMaxUsers()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 0;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const int userId = 1;
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Create(name, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }

        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void UserDoesNotExistDoesNotAdd()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Never);
        }
        [Fact]
        public void UserDoesNotExistReturnsFalse()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void VisibilityPublicPassesPasswordAsNull()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const string password = "def";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }

        [Fact]
        public void VisibilityPublicPasswordNullPasses()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));

            //act
            sut.Create(name, maxUsers, visibility, null, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.Is<ChatGroup>(g => g.Name.Equals(name) && g.MaxUsers.Equals(maxUsers) && g.Visibility.Equals(visibility) && g.PasswordSalt==null && g.Password==null && g.ModeratorId.Equals(userId))), Times.Once);
        }
        [Fact]
        public void VisibilityPublicPasswordNullReturnsTrue()
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Public;
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));

            //act
            bool result = sut.Create(name, maxUsers, visibility, null, userId, groupUserServiceMock.Object).Result;

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
            int? groupId = 1;
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Never);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void NameEmptyOrNullReturnsFalse(string name)
        {
            //arrange
            int? groupId = 1;
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string password = "def";
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Result;

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
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroup>()), Times.Never);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void VisibilityPrivatePasswordEmptyOrNullReturnsFalse(string password)
        {
            //arrange
            int? groupId = 1;
            const string name = "abc";
            const int maxUsers = 5;
            const ChatGroupVisibility visibility = ChatGroupVisibility.Private;
            const string passwordSalt = "ghi";
            const string passwordHashed = "jkl";
            const int userId = 1;
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(m => m.Add(It.IsAny<ChatGroup>())).Returns(Task.FromResult(groupId));
            hashAndSaltServiceMock.Setup(m => m.GenerateSalt()).Returns(passwordSalt);
            hashAndSaltServiceMock.Setup(m => m.Hash(password, passwordSalt)).Returns(passwordHashed);

            //act
            bool result = sut.Create(name, maxUsers, visibility, password, userId, groupUserServiceMock.Object).Result;

            //assert
            Assert.False(result);
        }
    }
}
