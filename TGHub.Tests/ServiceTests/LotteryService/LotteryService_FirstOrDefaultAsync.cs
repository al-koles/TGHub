using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.SqlDb;

namespace TGHub.Tests.ServiceTests.LotteryService;

public class LotteryService_FirstOrDefaultAsync
{
    [Fact]
    public async Task FirstOrDefaultAsync_NoPredicate_ReturnsFirstLottery()
    {
        // Arrange
        var dbContext = GetDbContextWithMockedData();
        var lotteryService = new Application.Services.Lotteries.LotteryService(dbContext);

        // Act
        var result = await lotteryService.FirstOrDefaultAsync();

        // Assert
        Assert.Equal("Lottery 1", result?.Title);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithPredicate_ReturnsMatchingLottery()
    {
        // Arrange
        var dbContext = GetDbContextWithMockedData();
        var lotteryService = new Application.Services.Lotteries.LotteryService(dbContext);
        Expression<Func<Lottery, bool>> predicate = l => l.Title == "Lottery 2";

        // Act
        var result = await lotteryService.FirstOrDefaultAsync(predicate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Lottery 2", result.Title);
    }

    // Helper method to create a mock DbContext with test data
    private ITgHubDbContext GetDbContextWithMockedData()
    {
        var options = new DbContextOptionsBuilder<TgHubDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        var dbContext = new TgHubDbContext(options);

        var lotteries = new List<Lottery>
        {
            new Lottery { Title = "Lottery 1", StartDateTime = new DateTime(2023, 2, 1), EndDateTime = new DateTime(2023, 2, 28), CreatorId = 1 },
            new Lottery { Title = "Lottery 2", StartDateTime = new DateTime(2023, 3, 1), EndDateTime = new DateTime(2023, 3, 31), CreatorId = 2 },
            new Lottery { Title = "Lottery 3", StartDateTime = new DateTime(2023, 4, 1), EndDateTime = new DateTime(2023, 4, 30), CreatorId = 1 }
        };

        dbContext.Lotteries.AddRange(lotteries);
        dbContext.SaveChanges();

        return dbContext;
    }
}