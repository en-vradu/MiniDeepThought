using MiniDeepThought.src.MiniDeepThought.Domain;

namespace MiniDeepThought.src.MiniDeepThought.Strategies;

public interface IAnswerStrategies
{
    /// <summary>
    /// Executes the algorithm. Reports progress 0..100 and honors cancellation.
    /// Returns a JobResult when complete.
    /// </summary>
    Task<JobResult> ExecuteAsync(Job job, IProgress<int> progress, CancellationToken token);
}
