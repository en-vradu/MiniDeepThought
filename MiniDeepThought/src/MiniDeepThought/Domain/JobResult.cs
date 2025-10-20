namespace MiniDeepThought.src.MiniDeepThought.Domain;

public class JobResult
{
    public string Answer { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public long DurationMs { get; set; }
}
