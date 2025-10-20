using MiniDeepThought.src.MiniDeepThought.Domain;
using MiniDeepThought.src.MiniDeepThought.Services;
using MiniDeepThought.src.MiniDeepThought.Strategies;
using MiniDeepThought.src.MiniDeepThought.Util;
using System.Text.Json;

namespace MiniDeepThought
{
    internal class Program
    {
        private static readonly JobStore _jobStore = new("deepthought-jobs.json");
        private static readonly JobRunner _jobRunner = new(_jobStore);

        private static async Task Main(string[] args)
        {
            Console.WriteLine("=== Mini Deep Thought ===");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("1) Submit Question");
                Console.WriteLine("2) List Jobs");
                Console.WriteLine("3) View Result by JobId");
                Console.WriteLine("4) Cancel Running Job");
                Console.WriteLine("5) Exit");
                Console.Write("Select: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await SubmitQuestionAsync();
                        break;
                    case "2":
                        ListJobs();
                        break;
                    case "3":
                        ViewResult();
                        break;
                    case "4":
                        CancelJob();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private static async Task SubmitQuestionAsync()
        {
            Console.Write("Enter your Ultimate Question (1–200 chars): ");
            var question = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(question) || question.Length > 200)
            {
                Console.WriteLine("Invalid question. Try again.");
                return;
            }

            Console.Write("Algorithm [Trivial|SlowCount|RandomGuess]: ");
            var algoKey = Console.ReadLine()?.Trim();

            IAnswerStrategies? strategy = algoKey switch
            {
                "Trivial" => new TrivialStrategy(),
                "SlowCount" => new SlowCountStrategy(),
                "RandomGuess" => new RandomCountStrategy(),
                _ => null
            };

            if (strategy == null)
            {
                Console.WriteLine("Unknown algorithm. Try again.");
                return;
            }

            var job = new Job
            {
                JobId = Guid.NewGuid(),
                QuestionText = question,
                AlgorithmKey = algoKey!,
                CreatedUtc = DateTime.UtcNow,
                Status = JobStatus.Pending
            };

            await _jobStore.AddOrUpdateAsync(job);

            Console.WriteLine($"Job queued: {job.JobId}");
            Console.WriteLine("Press 'C' to cancel.");

            var progress = new Progress<int>(p => Console.Write($"\rProgress: {p}%   "));

            // run job asynchronously (awaited to prevent multiple jobs at once)
            var runTask = _jobRunner.RunJobAsync(job, strategy, progress);

            // watch for 'C' while the job is running
            while (!runTask.IsCompleted)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
                {
                    _jobRunner.CancelCurrentJob();
                    Console.WriteLine("\nCancel requested...");
                }
                await Task.Delay(100);
            }

            Console.WriteLine();
            Console.WriteLine($"Job finished with status: {job.Status}");
        }

        private static void ListJobs()
        {
            var jobs = _jobStore.GetAll();

            if (!jobs.Any())
            {
                Console.WriteLine("No jobs found.");
                return;
            }

            Console.WriteLine("JobId | Status | Algorithm | CreatedUtc | Progress");
            foreach (var job in jobs)
            {
                Console.WriteLine($"{job.JobId} | {job.Status} | {job.AlgorithmKey} | {job.CreatedUtc:u} | {job.Progress}%");
            }
        }

        private static void ViewResult()
        {
            Console.Write("Enter JobId: ");
            var idText = Console.ReadLine();

            if (!Guid.TryParse(idText, out var jobId))
            {
                Console.WriteLine("Invalid JobId.");
                return;
            }

            var job = _jobStore.GetById(jobId);
            if (job == null)
            {
                Console.WriteLine("Job not found.");
                return;
            }

            if (job.Status != JobStatus.Completed || job.Result == null)
            {
                Console.WriteLine("Job is not completed yet.");
                return;
            }

            Console.WriteLine(JsonSerializer.Serialize(new
            {
                job.JobId,
                job.Result.Answer,
                job.Result.Summary,
                job.Result.DurationMs
            }, new JsonSerializerOptions { WriteIndented = true }));
        }

        private static void CancelJob()
        {
            if (!_jobRunner.IsJobRunning)
            {
                Console.WriteLine("No job is currently running.");
                return;
            }

            _jobRunner.CancelCurrentJob();
            Console.WriteLine("Cancellation requested.");
        }
    }
}