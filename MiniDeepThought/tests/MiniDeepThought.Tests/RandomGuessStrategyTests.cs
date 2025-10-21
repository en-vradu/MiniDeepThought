using MiniDeepThought.src.MiniDeepThought.Domain;
using MiniDeepThought.src.MiniDeepThought.Strategies;

namespace MiniDeepThought.Tests
{
    public class RandomGuessStrategyTests
    {
        [Fact]
        public async Task RandomGuessStrategy_ProducesNumberString()
        {
            // Arrange
            var strategy = new RandomCountStrategy();
            var job = new Job
            {
                JobId = Guid.NewGuid(),
                QuestionText = "Guess?",
                AlgorithmKey = "RandomGuess",
                CreatedUtc = DateTime.UtcNow
            };

            var reported = new List<int>();
            var progress = new Progress<int>(p => reported.Add(p));
            using var cts = new CancellationTokenSource();

            // Act
            var result = await strategy.ExecuteAsync(job, progress, cts.Token);

            // Assert
            Assert.True(int.TryParse(result.Answer, out var n), "Answer was not a number string.");
            Assert.Equal(42, n);
            Assert.Equal("Random guess from pool.", result.Summary);
            Assert.True(result.DurationMs > 0);

            Assert.True(reported.Count >= 2);
            Assert.Equal(0, reported[0]);
            Assert.Equal(100, reported[^1]);
            for (int i = 1; i < reported.Count; i++)
                Assert.True(reported[i] >= reported[i - 1], $"Progress decreased: {reported[i - 1]} -> {reported[i]}");
        }
    }
}
