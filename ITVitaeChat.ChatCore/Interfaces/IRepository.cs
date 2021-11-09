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
        Task Add(T value);
        Task Edit(T value);
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> query);
        Task<bool> Contains(Expression<Func<T, bool>> query);
    }
}
