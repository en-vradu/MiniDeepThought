using MiniDeepThought.src.MiniDeepThought.Domain;
using System.Text.Json;

namespace MiniDeepThought.src.MiniDeepThought.Services;

public class JobStore
{
    private readonly string _filePath;
    private readonly List<Job> _jobs = new();

    public JobStore(string filePath = "deepthought-jobs.json")
    {
        _filePath = filePath;

        Load();
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var jobs = JsonSerializer.Deserialize<List<Job>>(json);

            if (jobs != null)
            {
                _jobs.AddRange(jobs);
            }
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_jobs, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(_filePath, json);
    }

    public IReadOnlyList<Job> GetAll() => _jobs.AsReadOnly();

    public Job? GetById(Guid jobId) => _jobs.FirstOrDefault(j => j.JobId == jobId);

    public Task AddOrUpdateAsync(Job job)
    {
        var index = _jobs.FindIndex(j => j.JobId == job.JobId);

        if (index >= 0)
        {
            _jobs[index] = job;
        }

        else
        {
            _jobs.Add(job);
        }

        Save();

        return Task.CompletedTask;
    }
}