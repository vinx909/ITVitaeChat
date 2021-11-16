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
    public class Add : ChatgroupUserServiceTestBase
    {
        [Fact]
        public void CorrectPasses()
        {
            //arrange
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatgroupUser>()), Times.Once);
            groupUserRepositoryMock.Verify(m => m.Add(It.Is<ChatgroupUser>(gu => gu.ChatgroupId == groupId && gu.UserId == userId)), Times.Once);
        }
        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
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
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(false));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatgroupUser>()), Times.Never);
        }
        [Fact]
        public void UserDoesNotExistsReturnsFalse()
        {
            //arrange
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
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
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(false));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatgroupUser>()), Times.Never);
        }
        [Fact]
        public void GroupDoesNotExistsReturnsFalse()
        {
            //arrange
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(false));
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
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatgroupUser, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Add(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Add(It.IsAny<ChatgroupUser>()), Times.Never);
        }
        [Fact]
        public void GroupUserAlreadyExistsReturnsFalse()
        {
            //arrange
            const uint groupId = 1;
            const uint userId = 2;
            groupServiceMock.Setup(m => m.Exist(groupId)).Returns(Task.FromResult(true));
            userServiceMock.Setup(m => m.Exists(userId)).Returns(Task.FromResult(true));
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatgroupUser, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(groupId, userId).Result;

            //assert
            Assert.False(result);
        }
    }
}
