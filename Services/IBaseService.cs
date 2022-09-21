using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBaseService<T> where T : class, IEntity, new()
    {
        Task CreateAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<ICollection<T>> GetAllAsync();
        Task<int> SaveAsync();
    }
}
