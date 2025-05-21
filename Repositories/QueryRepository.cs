using DataTransform.Data;
using DataTransform.Interfaces;
using DataTransform.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Repositories;

public class QueryRepository : IQueryRepository
{
    private readonly AppDbContext _context;

    public QueryRepository(AppDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }
    
    public async Task<TerminalData?> GetVehicleIdQueryMapping(string? terminalId)
    {
        if (string.IsNullOrWhiteSpace(terminalId))
        {
            return null;
        }
        var result = await _context.TerminalData
            .FirstOrDefaultAsync(t => t.TerminalId == terminalId);
        
        return result;
    }
}