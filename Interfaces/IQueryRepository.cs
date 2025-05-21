using DataTransform.Models;

namespace DataTransform.Interfaces;

public interface IQueryRepository
{
    Task<TerminalData?> GetVehicleIdQueryMapping(string? terminalId);
}