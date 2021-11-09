using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.AdministratorService
{
    public class Get : AdministratorServiceTastBase
    {
        [Fact]
        public void ReturnsAdministrator()
        {
            //arrange
            const int id = 1;
            Administrator administrator = new();
            administratorRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(administrator));

            //act
            Administrator result = sut.Get(id).Result;

            //assert
            Assert.Equal(administrator, result);
        }

        [Fact]
        public void ReturnsNullOnNoAdministrator()
        {
            //arrange
            const int id = 1;
            administratorRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<Administrator>(null));

            //act
            Administrator result = sut.Get(id).Result;

            //assert
            Assert.Null(result);
        }
    }
}
