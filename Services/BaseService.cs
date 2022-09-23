using DataAccess.Contexts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Services
{
    public class BaseService<T> : IBaseService<T> where T : class, IEntity, new()
    {
        public BaseService() { }

        private readonly AppDbContext _dbContext;

        public BaseService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public DbSet<T> Table => _dbContext.Set<T>();
        public async Task CreateAsync(T entity) => await Table.AddAsync(entity);
        public void Delete(T entity) => Table.Remove(entity);
        public void Update(T entity) => Table.Update(entity);
        public async Task<ICollection<T>> GetAllAsync() => await Table.ToListAsync();
        public async Task<T> GetAsync(Expression<Func<T, bool>> expression) => await Table.Where(expression).FirstOrDefaultAsync();
        public async Task<int> SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
