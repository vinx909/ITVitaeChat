using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITVitaeChat.ChatInfrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private const string exceptionMessageDoesNotContainId = "the object is of type {0} and does not contain the propperty {1} which is required for the method {2} in the repository";

        private const string proppertyNameId = "Id";

        protected readonly ITVitaeChatDbContext dbContext;

        public Repository(ITVitaeChatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> Add(T value)
        {
            dbContext.Add(value);
            await dbContext.SaveChangesAsync();
            if(typeof(T).GetProperty(proppertyNameId) != null)
            {
                return (int) typeof(T).GetProperty(proppertyNameId).GetValue(value);
            }
            else
            {
                return null;
            }
        }

        public async Task Update(T value)
        {
            dbContext.Update(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(T value)
        {
            dbContext.Remove(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Contains(int id)
        {
            if (typeof(T).GetProperty(proppertyNameId) != null)
            {
                return await dbContext.Set<T>().AnyAsync(t => t.GetType().GetProperty(proppertyNameId).GetValue(t).Equals(id));
            }
            else
            {
                throw new InvalidCastException(string.Format(exceptionMessageDoesNotContainId, typeof(T), proppertyNameId, nameof(Contains)));
            }
        }
        public async Task<bool> Contains(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return await dbContext.Set<T>().AnyAsync(query);
        }

        public async Task<T> Get(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return await dbContext.Set<T>().SingleOrDefaultAsync(query);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.Set<T>().ToArrayAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> query)
        {
            return await dbContext.Set<T>().Where(query).ToArrayAsync();
        }
    }
}
