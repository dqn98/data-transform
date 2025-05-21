using DataTransform.Helpers;
using DataTransform.Interfaces;
using DataTransform.Models;

namespace DataTransform.Services;

public class EventService : IEventService
{
    private readonly IDataRepository<RawUserEvent> _rawRepository;
    private readonly ILogger<EventService> _logger;
    public EventService(IDataRepository<RawUserEvent> rawRepository, ILogger<EventService> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(rawRepository);

        _logger = logger;
        _rawRepository = rawRepository;
    }

    public async Task<long> SaveEvent(JsonEvent jsonEvent, CancellationToken cancellationToken = default)
    {
        var entity = jsonEvent.MapToRawUserEvent();

        await _rawRepository.AddAsync(entity, cancellationToken);
        await _rawRepository.SaveChangesAsync(cancellationToken); // Save processed events
        _logger.LogInformation($"Successfully save event: {entity.Id}.");
        
        return entity.Id;
    }
}