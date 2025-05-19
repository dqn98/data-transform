using System.Linq.Expressions;
using DataTransform.Data;
using DataTransform.Interfaces;
using DataTransform.Models;

namespace DataTransform.Repositories
{
    public class RawDataRepository(RawDbContext context) : IDataRepository<RawUserEvent>
    {
        public IQueryable<RawUserEvent> GetAllAsync()
        {
            return context.RawUserEvents.AsQueryable();
        }

        public IQueryable<RawUserEvent> FindAsync(Expression<Func<RawUserEvent, bool>> predicate)
        {
            return context.RawUserEvents.Where(predicate).AsQueryable();
        }

        public async Task AddAsync(RawUserEvent entity, CancellationToken cancellationToken = default)
        {
            await context.RawUserEvents.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<RawUserEvent> entities, CancellationToken cancellationToken = default)
        {
            await context.RawUserEvents.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(RawUserEvent entity, CancellationToken cancellationToken = default)
        {
             context.RawUserEvents.Update(entity);
        }

        public void UpdateRange(IEnumerable<RawUserEvent> entities, CancellationToken cancellationToken = default)
        {
            context.RawUserEvents.UpdateRange(entities);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}