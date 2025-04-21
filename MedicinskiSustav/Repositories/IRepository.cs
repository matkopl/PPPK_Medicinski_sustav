using MedicinskiSustav.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinskiSustav.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly MedicinskiDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MedicinskiDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()  => _dbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task InsertAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task UpdateAsync(T entity) => _dbSet.Update(entity);

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
