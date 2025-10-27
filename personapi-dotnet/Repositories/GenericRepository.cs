using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;
using System.Linq.Expressions;

namespace personapi_dotnet.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly PersonaDbContext _ctx;
        protected readonly DbSet<T> _set;
        public GenericRepository(PersonaDbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public Task<List<T>> GetAllAsync() => _set.AsNoTracking().ToListAsync();
        public Task<T?> GetByIdAsync(object id) => _set.FindAsync(id).AsTask();

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            _set.AsNoTracking().Where(predicate).ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            _set.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return;
            _set.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
