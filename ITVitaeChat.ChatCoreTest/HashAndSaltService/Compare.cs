using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.HashAndSaltService
{
    public class Compare : HashAndSaltServiceTestBase
    {
        [Fact]
        public void SameReturnsTrue()
        {
            //arrange
            string sameString = "abc123";
            string salt = sut.GenerateSalt();
            string sameStringHashed = sut.Hash(sameString, salt);

            //act
            bool result = sut.Compare(sameString, sameStringHashed, salt);

            //assert
            Assert.True(result);
        }
        [Fact]
        public void DifferentReturnsFalse()
        {
            //arrange
            string differentStringOne = "abc123";
            string differentStrintTwo = "def456";
            string salt = sut.GenerateSalt();
            string sameStringHashed = sut.Hash(differentStrintTwo, salt);

            //act
            bool result = sut.Compare(differentStringOne, sameStringHashed, salt);

            //assert
            Assert.False(result);
        }
    }
}
