using MiniDeepThought.src.MiniDeepThought.Domain;

namespace MiniDeepThought.src.MiniDeepThought.Strategies;

public class SlowCountStrategy : IAnswerStrategies
{
    private readonly int _steps = 100;
    private readonly int _delayPerStepMs = 50;

    public async Task<JobResult> ExecuteAsync(Job job, IProgress<int> progress, CancellationToken token)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i <= _steps; i++)
        {
            token.ThrowIfCancellationRequested();

            var percent = (int)Math.Round(i * 100.0 / _steps);

            progress.Report(percent);

            await Task.Delay(_delayPerStepMs, token);
        }

        sw.Stop();

        return new JobResult { Answer = "42", Summary = "Slow count.", DurationMs = sw.ElapsedMilliseconds };
    }
}
