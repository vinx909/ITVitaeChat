using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatgroupService
{
    public class Exists : ChatGroupServiceTestBase
    {
        [Fact]
        public void IfExistsReturnsTrue()
        {
            //arrange
            const int id = 1;
            groupRepositoryMock.Setup(m => m.Contains(id)).Returns(Task.FromResult(true));

            //act
            bool result = sut.Exists(id).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IfDoesNotExistsReturnsFalse()
        {
            //arrange
            const int id = 1;
            groupRepositoryMock.Setup(m => m.Contains(id)).Returns(Task.FromResult(false));

            //act
            bool result = sut.Exists(id).Result;

            //assert
            Assert.False(result);
        }
    }
}
