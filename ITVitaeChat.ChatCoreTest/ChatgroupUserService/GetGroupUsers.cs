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
    public class GetGroupUsers : ChatGroupUserServiceTestBase
    {
        [Fact]
        public void CorrectReturnUserIdIEnumberable()
        {
            //arrange
            const int groupId = 1;
            IEnumerable<int> idList = new int[] { 2, 3, 4 };
            groupUserRepositoryMock.Setup(m => m.GetAll(It.IsAny<Expression<Func<ChatGroupUser, bool>>>())).Returns(Task.FromResult(CreateGroupUsers(groupId, idList)));

            //act
            IEnumerable<int> result = sut.GetGroupUsers(groupId).Result;

            //assert
            Assert.True(IEnumberableMatches(idList, result));
        }

        private IEnumerable<ChatGroupUser> CreateGroupUsers(int groupId, IEnumerable<int> userIds)
        {
            List<ChatGroupUser> chatGroupUsers = new();
            foreach (int userId in userIds)
            {
                chatGroupUsers.Add(new() { ChatgroupId = groupId, UserId = userId });
            }
            return chatGroupUsers;
        }
        private bool IEnumberableMatches<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            IEnumerator<T> enumeratorExpected = expected.GetEnumerator();
            IEnumerator<T> enumeratorActual = actual.GetEnumerator();
            while (enumeratorExpected.MoveNext())
            {
                if (!enumeratorActual.MoveNext())
                {
                    return false;
                }
                if (enumeratorExpected.Current == null)
                {
                    if(!(enumeratorActual.Current == null))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!enumeratorExpected.Current.Equals(enumeratorActual.Current))
                    {
                        return false;
                    }
                }
            }
            if(enumeratorActual.MoveNext() == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
