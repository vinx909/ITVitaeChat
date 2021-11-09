using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.AdministratorService
{
    public class Add : AdministratorServiceTastBase
    {
        [Fact]
        public void AddAdministrator()
        {
            //arrange
            Administrator administrator = new() { UserId = 1 };

            //act
            sut.Add(administrator).Wait();

            //assert
            administratorRepositoryMock.Verify(m => m.Add(administrator), Times.Once);
        }
        [Fact]
        public void ReturnTrueOnSuccess()
        {
            //arrange
            Administrator administrator = new() { UserId = 1 };

            //act
            bool result = sut.Add(administrator).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void DoesNotAddAdministratorThatAlreadyExists()
        {
            //arrange
            const int id = 1;
            Administrator administrator = new() { UserId = id };
            administratorRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<Administrator, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Add(administrator).Wait();

            //assert
            administratorRepositoryMock.Verify(m => m.Add(administrator), Times.Never);
        }
        [Fact]
        public void ReturnFalseOnAdministratorThatAlreadyExists()
        {
            //arrange
            const int id = 1;
            Administrator administrator = new() { UserId = id };
            administratorRepositoryMock.Setup(m => m.Contains(It.IsAny<Expression<Func<Administrator, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Add(administrator).Result;

            //assert
            Assert.False(result);
        }
    }
}
