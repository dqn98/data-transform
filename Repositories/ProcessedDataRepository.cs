using System.Linq.Expressions;
using DataTransform.Core.Models;
using DataTransform.Data;
using DataTransform.Interfaces;

namespace DataTransform.Repositories
{
    public class ProcessedDataRepository(AppDbContext context) : IDataRepository<UserEvent>
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
            // Ensure all DateTime values are UTC
            if (entity.EventTimestamp.Kind != DateTimeKind.Utc)
                entity.EventTimestamp = DateTime.SpecifyKind(entity.EventTimestamp, DateTimeKind.Utc);
                
            await context.UserEvents.AddAsync(entity, cancellationToken);
        }
        
        public void Update(UserEvent entity, CancellationToken cancellationToken = default)
        {
            // Ensure all DateTime values are UTC
            if (entity.EventTimestamp.Kind != DateTimeKind.Utc)
                entity.EventTimestamp = DateTime.SpecifyKind(entity.EventTimestamp, DateTimeKind.Utc);
                
            context.UserEvents.Update(entity);
        }

        public void UpdateRange(IEnumerable<UserEvent> entities, CancellationToken cancellationToken = default)
        {
            // Ensure all DateTime values are UTC
            foreach (var entity in entities)
            {
                if (entity.EventTimestamp.Kind != DateTimeKind.Utc)
                    entity.EventTimestamp = DateTime.SpecifyKind(entity.EventTimestamp, DateTimeKind.Utc);
            }
            
            context.UserEvents.UpdateRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<UserEvent> entities, CancellationToken cancellationToken = default)
        {
            // Ensure all DateTime values are UTC
            foreach (var entity in entities)
            {
                if (entity.EventTimestamp.Kind != DateTimeKind.Utc)
                    entity.EventTimestamp = DateTime.SpecifyKind(entity.EventTimestamp, DateTimeKind.Utc);
            }
            
            await context.UserEvents.AddRangeAsync(entities, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}