using DataTransform.Models;

namespace DataTransform.Interfaces;

public interface IEventService
{
    Task<long> SaveEvent(JsonEvent jsonEvent, CancellationToken cancellationToken = default);
}