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
    public class Exists : ChatgroupUserServiceTestBase
    {
        [Fact]
        public void IfExistsReturnsTrue()
        {
            //arrange
            const uint groupId = 2;
            const uint userId = 1;
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatgroupUser, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Exists(groupId, userId).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfDoesNotExistsReturnsFalse()
        {
            //arrange
            const uint groupId = 2;
            const uint userId = 1;
            groupUserRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<ChatgroupUser, bool>>>())).Returns(Task.FromResult(false));

            //act
            bool result = sut.Exists(groupId, userId).Result;

            //assert
            Assert.False(result);
        }
    }
}
