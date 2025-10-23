using MiniDeepThought.src.MiniDeepThought.Domain;
using MiniDeepThought.src.MiniDeepThought.Strategies;
using MiniDeepThought.src.MiniDeepThought.Util;

namespace MiniDeepThought.src.MiniDeepThought.Services;

public class JobRunner
{
    private readonly JobStore _store;
    private CancellationTokenSource? _currentCts;
    private Job? _runningJob;

    public JobRunner(JobStore store)
    {
        _store = store;
    }

    public async Task RunJobAsync(Job job, IAnswerStrategies strategy, IProgress<int>? progress = null)
    {
        if (_currentCts != null)
        {
            throw new InvalidOperationException("Only one job can run at a time.");
        }
            
        _currentCts = new CancellationTokenSource();
        _runningJob = job;

        job.Status = JobStatus.Running;
        job.StartedUtc = DateTime.UtcNow;
        job.Progress = 0;

        await _store.AddOrUpdateAsync(job);

        try
        {
            var result = await strategy.ExecuteAsync(job, progress, _currentCts.Token);

            job.Status = JobStatus.Completed;
            job.Result = result;
            job.Progress = 100;
            job.CompletedUtc = DateTime.UtcNow;

            await _store.AddOrUpdateAsync(job);
        }
        catch (OperationCanceledException)
        {
            job.Status = JobStatus.Canceled;

            await _store.AddOrUpdateAsync(job);
        }
        finally
        {
            _currentCts.Dispose();
            _currentCts = null;
            _runningJob = null;
        }
    }

    public void CancelCurrentJob()
    {
        _currentCts?.Cancel();
    }

    public bool IsJobRunning => _currentCts != null;
}
