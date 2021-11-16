using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;

namespace ITVitaeChat.ChatCore.Interfaces
{
    public interface IRepository<T>
    {
        Task<uint?> Add(T value);
        Task Update(T value);
        Task Delete(T value);
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(uint id);
        Task<T> Get(Expression<Func<T, bool>> query);
        Task<bool> Contains(uint id);
        Task<bool> Contains(Expression<Func<T, bool>> query);
    }
}
