using DataTransform.Core.Interfaces;
using DataTransform.Core.Models;
using DataTransform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataTransform.Infrastructure.Repositories
{
    public class ProcessedDataRepository : IDataRepository<UserEvent>
    {
        private readonly ProcessedDbContext _context;

        public ProcessedDataRepository(ProcessedDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEvent>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserEvents.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserEvent>> FindAsync(Expression<Func<UserEvent, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.UserEvents.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(UserEvent entity, CancellationToken cancellationToken = default)
        {
            await _context.UserEvents.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<UserEvent> entities, CancellationToken cancellationToken = default)
        {
            await _context.UserEvents.AddRangeAsync(entities, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}