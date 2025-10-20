using MiniDeepThought.src.MiniDeepThought.Domain;

namespace MiniDeepThought.src.MiniDeepThought.Strategies;

public class RandomCountStrategy : IAnswerStrategies
{
    private static readonly int[] _pool = new[] { 42 };

    public async Task<JobResult> ExecuteAsync(Job job, IProgress<int> progress, CancellationToken token)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        progress.Report(0);

        for (int i = 0; i < 5; i++)
        {
            token.ThrowIfCancellationRequested();

            await Task.Delay(200, token);

            progress.Report((i + 1) * 20);
        }

        var rnd = new Random();
        var pick = _pool[rnd.Next(_pool.Length)];

        sw.Stop();

        return new JobResult { Answer = pick.ToString(), Summary = "Random guess from pool.", DurationMs = sw.ElapsedMilliseconds };
    }
}
