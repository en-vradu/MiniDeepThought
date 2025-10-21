using MiniDeepThought.src.MiniDeepThought.Domain;
using MiniDeepThought.src.MiniDeepThought.Strategies;

namespace MiniDeepThought.tests.MiniDeepThought.Tests;

public class SlowCountStrategyTests
{
    [Fact]
    public async Task SlowCountStrategy_AdvancesProgress_To100()
    {
        // Arrange
        var strategy = new SlowCountStrategy();
        var job = new Job
        {
            JobId = Guid.NewGuid(),
            QuestionText = "Ultimate?",
            AlgorithmKey = "SlowCount",
            CreatedUtc = DateTime.UtcNow
        };

        var reported = new List<int>();
        var progress = new Progress<int>(p => reported.Add(p));
        using var cts = new CancellationTokenSource();

        // Act
        var result = await strategy.ExecuteAsync(job, progress, cts.Token);

        // Assert
        Assert.Equal("42", result.Answer);
        Assert.Equal("Slow count.", result.Summary);
        Assert.True(result.DurationMs > 0);

        Assert.NotEmpty(reported);
        Assert.Equal(100, reported[^1]);                 
        for (int i = 1; i < reported.Count; i++)     
            Assert.True(reported[i] >= reported[i - 1], $"Progress decreased: {reported[i - 1]} -> {reported[i]}");
    }
}