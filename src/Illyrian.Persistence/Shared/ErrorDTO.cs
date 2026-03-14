namespace Illyrian.Persistence.Shared;

public class ErrorDTO
{
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
