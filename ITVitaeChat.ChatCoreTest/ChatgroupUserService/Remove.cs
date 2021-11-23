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
    public class Remove : ChatGroupUserServiceTestBase
    {
        [Fact]
        public void CorrectDeletes()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            ChatGroupUser groupUser = new() { ChatgroupId = groupId, UserId = userId };
            groupUserRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult(groupUser));

            //act
            sut.Remove(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Delete(groupUser), Times.Once);
        }
        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            ChatGroupUser groupUser = new() { ChatgroupId = groupId, UserId = userId };
            groupUserRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult(groupUser));

            //act
            bool result = sut.Remove(groupId, userId).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfGroupUserDoesNotExistDoesNotDelete()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            ChatGroupUser groupUser = new() { ChatgroupId = groupId, UserId = userId };
            groupUserRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult<ChatGroupUser>(null));

            //act
            sut.Remove(groupId, userId).Wait();

            //assert
            groupUserRepositoryMock.Verify(m => m.Delete(It.IsAny<ChatGroupUser>()), Times.Never);
        }
        [Fact]
        public void IfGroupUserDoesNotExistReturnsFalse()
        {
            //arrange
            const int groupId = 1;
            const int userId = 2;
            ChatGroupUser groupUser = new() { ChatgroupId = groupId, UserId = userId };
            groupUserRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult<ChatGroupUser>(null));

            //act
            bool result = sut.Remove(groupId, userId).Result;

            //assert
            Assert.False(result);
        }
    }
}
