using MiniDeepThought.src.MiniDeepThought.Domain;
using MiniDeepThought.src.MiniDeepThought.Strategies;

namespace MiniDeepThought.Tests
{
    public class TrivialStrategyTests
    {
        [Fact]
        public async Task TrivialStrategy_Returns42_AndReports_0_Then_100()
        {
            var strategy = new TrivialStrategy();
            var job = new Job { JobId = Guid.NewGuid(), QuestionText = "Why?", AlgorithmKey = "Trivial", CreatedUtc = DateTime.UtcNow };
            var reported = new List<int>();
            var progress = new Progress<int>(p => reported.Add(p));
            using var cts = new CancellationTokenSource();

            var result = await strategy.ExecuteAsync(job, progress, cts.Token);
            await Task.Delay(10);

            Assert.Equal("42", result.Answer);
            Assert.Equal("Trivial answer.", result.Summary);
            Assert.True(result.DurationMs >= 0);

            Assert.True(reported.Count >= 2);
            Assert.Equal(0, reported[0]);
            Assert.Equal(100, reported[^1]);
        }
    }
}