using System.Linq.Expressions;

namespace SelectU.Contracts.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task DeleteAsync(object id);
        Task RemoveRange(ICollection<T> entities);
        Task<T?> GetAsync(object id);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> AsQueryable();
        void Detach(T entity);
        void DetachAll(List<T> entities);
        void Update(T entity);
        void AttachAndUpdateFields(T entity, List<string> fields);
    }
}
