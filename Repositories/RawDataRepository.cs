using DataTransform.Core.Interfaces;
using DataTransform.Core.Models;
using DataTransform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataTransform.Infrastructure.Repositories
{
    public class RawDataRepository : IDataRepository<RawUserEvent>
    {
        private readonly RawDbContext _context;

        public RawDataRepository(RawDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RawUserEvent>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.RawUserEvents.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RawUserEvent>> FindAsync(Expression<Func<RawUserEvent, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.RawUserEvents.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(RawUserEvent entity, CancellationToken cancellationToken = default)
        {
            await _context.RawUserEvents.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<RawUserEvent> entities, CancellationToken cancellationToken = default)
        {
            await _context.RawUserEvents.AddRangeAsync(entities, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}