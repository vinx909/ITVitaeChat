using ITVitaeChat.ChatCore.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.UserService
{
    public class Edit : UserServiceTestBase
    {
        [Fact]
        public void ValuesCorrectCallsEdit()
        {
            //arrange
            const uint id= 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, "def", "pqr", "ghi@jkl.mno").Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser));
        }
        [Fact]
        public void UserCorrectCallsEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, Name = "a", DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser));
        }
        [Fact]
        //the need for this may change. if that is the case all "User*" tests will need to be changed to extra details in the User user as they assume that the added details are unnesisary.
        public void UserWithoutUnnessisaryValuesCorrectCallsEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr"};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser));
        }
        [Fact]
        public void ValueCorrectReturnsTrue()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(id, "def", "pqr", "ghi@jkl.mno").Result;

            //assert
            Assert.True(result);
        }
        [Fact]
        public void UserCorrectReturnsTrue()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr"};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.True(result);
        }
        [Fact]
        //the need for this may change. if that is the case all "User*" tests will need to be changed to extra details in the User user as they assume that the added details are unnesisary.
        public void UserWithoutUnnessisaryValuesCorrectReturnsTrue()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void ValuesCorrectWithNullValuesCallsEdit(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, displayName, password, emailadress).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser));
        }
        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void UserCorrectWithNullValuesCallsEdit(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayName, Password = password, Emailadres = emailadress };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser));
        }
        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void ValuesCorrectWithNullValuesReturnsTrue(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(id, displayName, password, emailadress).Result;

            //assert
            Assert.True(result);
        }
        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void UserCorrectWithNullValuesReturnsTrue(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayName, Password = password, Emailadres = emailadress };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.True(result);
        }
        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void ValuesCorrectWithNullValuesDoesNotChangeValuesThatAreNull(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            const string oldDisplayName = "b";
            const string oldEmailadres = "c@d.ef";
            const string oldPassword = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = oldDisplayName, Emailadres = oldEmailadres, Password = oldPassword, PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, displayName, password, emailadress).Wait();

            //assert
            if(displayName == null)
            {
                Assert.Equal(oldDisplayName, repositoryUser.DisplayName);
            }
            else
            {
                Assert.Equal(displayName, repositoryUser.DisplayName);
            }
            if (emailadress == null)
            {
                Assert.Equal(oldEmailadres, repositoryUser.Emailadres);
            }
            else
            {
                Assert.Equal(emailadress, repositoryUser.Emailadres);
            }
            if (password == null)
            {
                Assert.Equal(oldPassword, repositoryUser.Password);
            }
        }
        [Theory]
        [InlineData("abc", "def", null)]
        [InlineData("abc", null, "g@h.ij")]
        [InlineData(null, "abc", "g@h.ij")]
        [InlineData("abc", null, null)]
        [InlineData(null, "abc", null)]
        [InlineData(null, null, "g@h.ij")]
        public void UserCorrectWithNullValuesDoesNotChangeValuesThatAreNull(string displayName, string password, string emailadress)
        {
            //arrange
            const uint id = 1;
            const string oldDisplayName = "b";
            const string oldEmailadres = "c@d.ef";
            const string oldPassword = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = oldDisplayName, Emailadres = oldEmailadres, Password = oldPassword, PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayName, Password = password, Emailadres = emailadress };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            if (displayName == null)
            {
                Assert.Equal(oldDisplayName, repositoryUser.DisplayName);
            }
            else
            {
                Assert.Equal(displayName, repositoryUser.DisplayName);
            }
            if (emailadress == null)
            {
                Assert.Equal(oldEmailadres, repositoryUser.Emailadres);
            }
            else
            {
                Assert.Equal(emailadress, repositoryUser.Emailadres);
            }
            if (password == null)
            {
                Assert.Equal(oldPassword, repositoryUser.Password);
            }
        }

        [Fact]
        public void ValuesAllNullDoesNotEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, null, null, null).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Fact]
        public void UserAllNullDoesNotEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = null, Emailadres = null, Password = null };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Fact]
        public void ValuesAllNullReturnsFalse()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(id, null, null, null).Result;

            //assert
            Assert.False(result);
        }
        [Fact]
        public void UserAllNullReturnsFalse()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = null, Emailadres = null, Password = null };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ValuesCorrectChangesDisplayName()
        {
            //arrange
            const uint id = 1;
            const string newDisplayName = "def";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, newDisplayName, "pqr", "ghi@jkl.mno").Wait();

            //assert
            Assert.Equal(newDisplayName, repositoryUser.DisplayName);
        }
        [Fact]
        public void UserCorrectChangesDisplayName()
        {
            //arrange
            const uint id = 1;
            const string newDisplayName = "def";
            const string emailadress = "c@d.ef";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = emailadress, Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = newDisplayName };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            Assert.Equal(newDisplayName, repositoryUser.DisplayName);
        }
        [Fact]
        public void ValuesCorrectChangesEmailadress()
        {
            //arrange
            const uint id = 1;
            const string newEmailadress = "def@ghi.jkl";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, null, null, newEmailadress).Wait();

            //assert
            Assert.Equal(newEmailadress, repositoryUser.Emailadres);
        }
        [Fact]
        public void UserCorrectChangesEmailadress()
        {
            //arrange
            const uint id = 1;
            const string newEmailadress = "def@ghi.jkl";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, Emailadres = newEmailadress };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            Assert.Equal(newEmailadress, repositoryUser.Emailadres);
        }


        [Fact]
        public void ValueNoReturnOnIdReturnsFalse()
        {
            //arrange
            const uint id = 1;
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<User>(null));

            //act
            bool result = sut.Edit(id, "def", "pqr", "ghi@jkl.mno").Result;

            //assert
            Assert.False(result);
        }
        [Fact]
        public void UserNoReturnOnIdReturnsFalse()
        {
            //arrange
            const uint id = 1;
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<User>(null));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "ghi@jkl.mno", "pqr")]
        [InlineData("def", "", "pqr")]
        [InlineData("def", " ", "pqr")]
        [InlineData("def", "   ", "pqr")]
        [InlineData("def", "ghi@jkl.mno", "")]
        [InlineData("def", "ghi@jkl.mno", " ")]
        [InlineData("def", "ghi@jkl.mno", "   ")]
        public void ValueIncompleteDataDoesNotEdit(string username, string emailadress, string password)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, username, password, emailadress).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Theory]
        [InlineData("", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "ghi@jkl.mno", "pqr")]
        [InlineData("def", "", "pqr")]
        [InlineData("def", " ", "pqr")]
        [InlineData("def", "   ", "pqr")]
        [InlineData("def", "ghi@jkl.mno", "")]
        [InlineData("def", "ghi@jkl.mno", " ")]
        [InlineData("def", "ghi@jkl.mno", "   ")]
        public void UserIncompleteDataDoesNotEdit(string username, string emailadress, string password)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = username, Emailadres = emailadress, Password = password};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Theory]
        [InlineData("", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "ghi@jkl.mno", "pqr")]
        [InlineData("def", "", "pqr")]
        [InlineData("def", " ", "pqr")]
        [InlineData("def", "   ", "pqr")]
        [InlineData("def", "ghi@jkl.mno", "")]
        [InlineData("def", "ghi@jkl.mno", " ")]
        [InlineData("def", "ghi@jkl.mno", "   ")]
        public void ValueIncompleteDataReturnsFalse(string username, string emailadress, string password)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(id, username, password, emailadress).Result;

            //assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("", "ghi@jkl.mno", "pqr")]
        [InlineData(" ", "ghi@jkl.mno", "pqr")]
        [InlineData("   ", "ghi@jkl.mno", "pqr")]
        [InlineData("def", "", "pqr")]
        [InlineData("def", " ", "pqr")]
        [InlineData("def", "   ", "pqr")]
        [InlineData("def", "ghi@jkl.mno", "")]
        [InlineData("def", "ghi@jkl.mno", " ")]
        [InlineData("def", "ghi@jkl.mno", "   ")]
        public void UserIncompleteDataReturnsFalse(string username, string emailadress, string password)
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = username, Emailadres = emailadress, Password = password};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void ValueIncorrectEmailadressDoesNotEdit(string emailadres)
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string password = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = "c@d.ef", Password = password, PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, displayname, password, emailadres).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(repositoryUser), Times.Never);
        }
        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void UserIncorrectEmailadressDoesNotEdit(string emailadres)
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string password = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = "c@d.ef", Password = password, PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayname, Emailadres = emailadres, Password = password};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Add(repositoryUser), Times.Never);
        }
        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void ValueIncorrectEmailadressReturnFalse(string emailadres)
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string password = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = "c@d.ef", Password = password, PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(id, displayname, password, emailadres).Result;

            //assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("abc")]
        [InlineData("@")]
        [InlineData("@.")]
        [InlineData("abc@.")]
        [InlineData("abc@def")]
        [InlineData("abc@def.")]
        [InlineData("abc@.def")]
        [InlineData("@abc.def")]
        [InlineData("abc@def.g")]
        [InlineData("abc@def.ghij")]
        public void UserIncorrectEmailadressReturnsFalse(string emailadres)
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string password = "g";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = "c@d.ef", Password = password, PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayname, Emailadres = emailadres, Password = password};
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ValueAlreadyExistingValuesDoesNotEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Edit(id, "def", "pqr", "ghi@jkl.mno").Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Fact]
        public void UserAlreadyExistingValuesDoesNotEdit()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            sut.Edit(user).Wait();

            //assert
            userRepositoryMock.Verify(m => m.Update(repositoryUser), Times.Never);
        }
        [Fact]
        public void ValueAlreadyExistingValuesReturnsFalse()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Edit(id, "def", "pqr", "ghi@jkl.mno").Result;

            //assert
            Assert.False(result);
        }
        [Fact]
        public void UserAlreadyExistingValuesReturnsFalse()
        {
            //arrange
            const uint id = 1;
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = "b", Emailadres = "c@d.ef", Password = "g", PasswordSalt = "h", Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = "def", Emailadres = "ghi@jkl.mno", Password = "pqr" };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            userRepositoryMock.Setup(m => m.Contains(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));

            //act
            bool result = sut.Edit(user).Result;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ValueHashingPasswordCallsInterface()
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string emailadress = "c@d.ef";
            const string passwordSalt = "hij";
            const string password = "klm";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = emailadress, Password = "g", PasswordSalt = passwordSalt, Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(id, displayname, password, emailadress).Wait();

            //assert
            hashAndSaltMock.Verify(m => m.Hash(password, passwordSalt), Times.Once);
        }
        [Fact]
        public void UserHashingPasswordCallsInterface()
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string emailadress = "c@d.ef";
            const string passwordSalt = "hij";
            const string password = "klm";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = emailadress, Password = "g", PasswordSalt = passwordSalt, Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayname, Emailadres = emailadress, Password = password };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));

            //act
            sut.Edit(user).Wait();

            //assert
            hashAndSaltMock.Verify(m => m.Hash(password, passwordSalt), Times.Once);
        }
        [Fact]
        public void ValueHashingPasswordAltersPassword()
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string emailadress = "c@d.ef";
            const string passwordSalt = "hij";
            const string password = "klm";
            const string hashedPassword = "nop";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = emailadress, Password = "g", PasswordSalt = passwordSalt, Validated = true, Blocked = false };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            hashAndSaltMock.Setup(m => m.Hash(password, passwordSalt)).Returns(hashedPassword);

            //act
            sut.Edit(id, displayname, password, emailadress).Wait();

            //assert
            Assert.Equal(hashedPassword, repositoryUser.Password);
        }
        [Fact]
        public void UserHashingPasswordAltersPassword()
        {
            //arrange
            const uint id = 1;
            const string displayname = "b";
            const string emailadress = "c@d.ef";
            const string passwordSalt = "hij";
            const string password = "klm";
            const string hashedPassword = "nop";
            User repositoryUser = new() { Id = id, Name = "a", DisplayName = displayname, Emailadres = emailadress, Password = "g", PasswordSalt = passwordSalt, Validated = true, Blocked = false };
            User user = new() { Id = id, DisplayName = displayname, Emailadres = emailadress, Password = password };
            userRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult(repositoryUser));
            hashAndSaltMock.Setup(m => m.Hash(user.Password, passwordSalt)).Returns(hashedPassword);

            //act
            sut.Edit(user).Wait();

            //assert
            Assert.Equal(hashedPassword, repositoryUser.Password);
        }
    }
}
