using MiniDeepThought.src.MiniDeepThought.Domain;

namespace MiniDeepThought.src.MiniDeepThought.Strategies;

public class TrivialStrategy : IAnswerStrategies
{
    public async Task<JobResult> ExecuteAsync(Job job, IProgress<int> progress, CancellationToken token)
    {
        progress.Report(0);
        progress.Report(100);

        var result = new JobResult { Answer = "42", Summary = "Trivial answer.", DurationMs = 0 };

        return result;
    }
}
