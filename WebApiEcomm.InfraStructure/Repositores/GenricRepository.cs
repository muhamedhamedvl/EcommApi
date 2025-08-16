using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.InfraStructure.Data;

namespace WebApiEcomm.InfraStructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(T entity)
        {
            if (entity is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(entity), "Entity cannot be null"));
            }
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }
        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();
        public Task<T> DeleteAsync(T entity)
        {
            if (entity is null)
            {
                return Task.FromException<T>(new ArgumentNullException(nameof(entity), "Entity cannot be null"));
            }
            _context.Remove(entity);
            return _context.SaveChangesAsync().ContinueWith(t => entity);
        }
        public Task<IReadOnlyList<T>> GetAllAsync()
        {
            return _context.Set<T>().ToListAsync().ContinueWith(t => (IReadOnlyList<T>)t.Result);
        }
        public Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.ToListAsync().ContinueWith(t => (IReadOnlyList<T>)t.Result);
        }
        public Task<T> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return Task.FromException<T>(new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero"));
            }
            return _context.Set<T>().FindAsync(id).AsTask();
        }
        public Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            if (id <= 0)
            {
                return Task.FromException<T>(new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero"));
            }

            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }
        public Task<T> UpdateAsync(T entity)
        {
            if (entity is null)
            {
                return Task.FromException<T>(new ArgumentNullException(nameof(entity), "Entity cannot be null"));
            }
            _context.Update(entity);
            return _context.SaveChangesAsync().ContinueWith(t => entity);
        }
    }
}
