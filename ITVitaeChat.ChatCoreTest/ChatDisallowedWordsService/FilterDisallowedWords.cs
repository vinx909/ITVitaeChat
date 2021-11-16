using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatDisallowedWordsService
{
    public class FilterDisallowedWords : ChatDisallowedWordsServiceTestBase
    {
        private const char maskingchar = '.';

        [Fact]
        public void DoesNotChangeWithoutDisallowedWords()
        {
            //arrange
            const string message = "abcdefg";

            //act
            string result = sut.FilterDisallowedWords(message).Result;

            //Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void DoesNotChangeWithoutDisallowedWordsWithRedHarring()
        {
            //arrange
            const string message = "abcdefg";
            disallowedWordsRepositoryMock.Setup(m => m.GetAll()).Returns(Task.FromResult((IEnumerable<ChatDisallowedWord>)new ChatDisallowedWord[] { new() { Content = "h" } }));

            //act
            string result = sut.FilterDisallowedWords(message).Result;

            //Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void RemovesSingularDisallowedWord()
        {
            //arrange
            const string message = "abcdefg";
            string expectedMessage = "a" + maskingchar + maskingchar + "defg";
            disallowedWordsRepositoryMock.Setup(m => m.GetAll()).Returns(Task.FromResult((IEnumerable<ChatDisallowedWord>)new ChatDisallowedWord[] { new() { Content = "h" }, new() { Content = "bc" } }));

            //act
            string result = sut.FilterDisallowedWords(message).Result;

            //Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void RemovesMultipleDisallowedWord()
        {
            //arrange
            const string message = "abcdefg";
            string expectedMessage = "a" + maskingchar + maskingchar + "de"+maskingchar+"g";
            disallowedWordsRepositoryMock.Setup(m => m.GetAll()).Returns(Task.FromResult((IEnumerable<ChatDisallowedWord>)new ChatDisallowedWord[] { new() { Content = "h" }, new() { Content = "bc" }, new() { Content="f" } }));

            //act
            string result = sut.FilterDisallowedWords(message).Result;

            //Assert
            Assert.Equal(expectedMessage, result);
        }
    }
}
