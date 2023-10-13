using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SelectU.Contracts.Repositories;
using SelectU.Migrations;

namespace SelectU.Core.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SelectUContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(SelectUContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(ICollection<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public async Task RemoveRange(ICollection<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T?> GetAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }
        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void DetachAll(List<T> entities)
        {
            foreach(var entity in entities)
            {
                Detach(entity);
            }
        }

        /// <summary>
        /// Attach disconnected entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fields">List of fields to update to the database</param>
        public void AttachAndUpdateFields(T entity, List<string> fields)
        {
            _dbSet.Attach(entity);
            foreach (var field in fields)
            {
                _context.Entry(entity).Property(field).IsModified = true;
            }
        }

        public void Update(T entity)
        {
            //  _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
