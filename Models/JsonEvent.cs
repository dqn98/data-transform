namespace DataTransform.Models;

public class JsonEvent
{
    public string? Type { get; set; }
    public string? Event { get; set; }
    public EventProperties? Properties { get; set; }
}