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
        Task<int?> Add(T value);
        Task Update(T value);
        Task Delete(T value);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> query);
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> query);
        Task<bool> Contains(int id);
        Task<bool> Contains(Expression<Func<T, bool>> query);
    }
}
