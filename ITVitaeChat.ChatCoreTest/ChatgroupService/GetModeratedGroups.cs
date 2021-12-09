using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Moq;
using Xunit;

namespace ITVitaeChat.ChatCoreTest.ChatgroupService
{
    public class GetModeratedGroups : ChatGroupServiceTestBase
    {
        public void ReturnsModeratedGroups()
        {
            //arrange
            const int userId = 1;
            IEnumerable<int> moderatedGroupIds = new int[] { 2, 3, 4 };
            IEnumerable<ChatGroup> moderatedGroups = CreateGroupsFromIds(moderatedGroupIds);
            groupRepositoryMock.Setup(m => m.GetAll(It.IsAny<Expression<Func<ChatGroup, bool>>>())).Returns(Task.FromResult(moderatedGroups));

            //act
            IEnumerable<ChatGroup> result = sut.GetModeratedGroups(userId).Result;

            //assert
            Assert.True(IEnumberableMatches(moderatedGroups, result));
        }

        private IEnumerable<ChatGroup> CreateGroupsFromIds(IEnumerable<int> ids)
        {
            List<ChatGroup> groups = new();
            foreach (int id in ids)
            {
                groups.Add(new() { Id = id });
            }
            return groups;
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
                    if (!(enumeratorActual.Current == null))
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
            if (enumeratorActual.MoveNext() == true)
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
