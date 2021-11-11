using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITVitaeChat.ChatInfrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ITVitaeChatDbContext dbContext;

        public Repository(ITVitaeChatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Add(T value)
        {
            dbContext.Add(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task Edit(T value)
        {
            dbContext.Update(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Contains(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return await dbContext.Set<T>().AnyAsync(query);
        }

        public async Task<T> Get(uint id)
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
        
    }
}
