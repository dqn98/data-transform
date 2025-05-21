namespace DataTransform.Interfaces
{
    public interface IDataTransformService
    {
        Task TransformDataAsync(CancellationToken cancellationToken = default);
    }
}