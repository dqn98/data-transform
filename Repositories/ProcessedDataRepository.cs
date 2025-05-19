using System.Linq.Expressions;
using DataTransform.Core.Models;
using DataTransform.Data;
using DataTransform.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Repositories
{
    public class ProcessedDataRepository(ProcessedDbContext context) : IDataRepository<UserEvent>
    {
        public IQueryable<UserEvent> GetAllAsync()
        {
            return context.UserEvents.AsQueryable();
        }

        public IQueryable<UserEvent> FindAsync(Expression<Func<UserEvent, bool>> predicate)
        {
            return context.UserEvents.Where(predicate).AsQueryable();
        }

        public async Task AddAsync(UserEvent entity, CancellationToken cancellationToken = default)
        {
            await context.UserEvents.AddAsync(entity, cancellationToken);
        }
        
        public void Update(UserEvent entity, CancellationToken cancellationToken = default)
        {
            context.UserEvents.Update(entity);
        }

        public void UpdateRange(IEnumerable<UserEvent> entities, CancellationToken cancellationToken = default)
        {
            context.UserEvents.UpdateRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<UserEvent> entities, CancellationToken cancellationToken = default)
        {
            await context.UserEvents.AddRangeAsync(entities, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}