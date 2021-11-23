using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatgroupService
{
    public class Remove : ChatGroupServiceTestBase
    {
        [Fact]
        public void CorrectRemovesGroup()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult(group));

            //act
            sut.Remove(groupId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Delete(It.IsAny<ChatGroup>()), Times.Once);
            groupRepositoryMock.Verify(m => m.Delete(group), Times.Once);
        }
        [Fact]
        public void CorrectReturnsTrue()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult(group));

            //act
            bool reuslt = sut.Remove(groupId, groupUserServiceMock.Object).Result;

            //assert
            Assert.True(reuslt);
        }

        [Fact]
        public void NotFoundDoesNotDelete()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult<ChatGroup>(null));

            //act
            sut.Remove(groupId, groupUserServiceMock.Object).Wait();

            //assert
            groupRepositoryMock.Verify(m => m.Delete(group), Times.Never);
        }
        [Fact]
        public void NotFoundReturnsFalse()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult<ChatGroup>(null));

            //act
            bool result = sut.Remove(groupId, groupUserServiceMock.Object).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void DoesNotFailWithGroupUserServiceNull()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult<ChatGroup>(null));

            //act
            sut.Remove(groupId, null).Wait();
        }
        [Fact]
        public void GroupUserServiceNullRetunsTrue()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult<ChatGroup>(group));

            //act
            bool result = sut.Remove(groupId, null).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void CorrectRemovesUsersFromGroupFirst()
        {
            //arrange
            const int groupId = 1;
            ChatGroup group = new();
            groupRepositoryMock.Setup(m => m.Get(groupId)).Returns(Task.FromResult(group));
            IEnumerable<int> idUserList = new int[] { 2, 3, 4 };
            groupUserServiceMock.Setup(m => m.GetGroupUsers(groupId)).Returns(Task.FromResult(idUserList));

            //act
            sut.Remove(groupId, groupUserServiceMock.Object).Wait();

            //assert
            groupUserServiceMock.Verify(m => m.Remove(groupId, It.IsAny<int>()), Times.Exactly(idUserList.Count()));
            foreach (int userId in idUserList)
            {
                groupUserServiceMock.Verify(m => m.Remove(groupId, userId), Times.Once);
            }
        }
    }
}
