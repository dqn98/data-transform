using System.Linq.Expressions;

namespace DataTransform.Interfaces
{
    public interface IDataRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}