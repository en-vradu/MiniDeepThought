using MiniDeepThought.src.MiniDeepThought.Util;

namespace MiniDeepThought.src.MiniDeepThought.Domain;

public class Job
{
    public Guid JobId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string AlgorithmKey { get; set; } = string.Empty;
    public JobStatus Status { get; set; }
    public int Progress { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime? StartedUtc { get; set; }
    public DateTime? CompletedUtc { get; set; }
    public JobResult? Result { get; set; }
}
