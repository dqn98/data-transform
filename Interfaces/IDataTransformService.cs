namespace DataTransform.Core.Interfaces
{
    public interface IDataTransformService
    {
        Task TransformDataAsync(CancellationToken cancellationToken = default);
    }
}