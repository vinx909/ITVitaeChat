using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatgroupUserService
{
    public class Add : ChatGroupUserServiceTestBase
    {
        [Fact]
        public void CorrectPasses()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroupUser>()), Times.Once);
            groupUserRepositoryMock.Verify(m => m.Add(It.Is<ChatGroupUser>(gu => gu.ChatgroupId == groupId && gu.UserId == userId)), Times.Once);
        }
        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(groupId, userId).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void UserDoesNotExistsDoesNotPass()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroupUser>()), Times.Never);
        }
        [Fact]
        public void UserDoesNotExistsReturnsFalse()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));

            //act
            bool result = sut.Add(groupId, userId).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GroupDoesNotExistsDoesNotPass()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(false));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroupUser>()), Times.Never);
        }
        [Fact]
        public void GroupDoesNotExistsReturnsFalse()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(false));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(groupId, userId).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GroupUserAlreadyExistsDoesNotPass()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatGroupUser>()), Times.Never);
        }
        [Fact]
        public void GroupUserAlreadyExistsReturnsFalse()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            groupServiceMock.Setup(m => m.Exists(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(groupId, userId).Result;

            //assert
            Assert.False(result);
        }
    }
}
