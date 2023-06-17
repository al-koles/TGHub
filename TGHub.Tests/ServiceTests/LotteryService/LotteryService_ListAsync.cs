using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Lotteries.Data;
using TGHub.Domain.Entities;
using TGHub.SqlDb;

namespace TGHub.Tests.ServiceTests.LotteryService;

public class LotteryService_ListAsync
{
    [Fact]
    public async Task ListAsync_NoFilter_ReturnsAllLotteries()
    {
        // Arrange
        var dbContext = GetDbContextWithMockedData();
        var lotteryService = new Application.Services.Lotteries.LotteryService(dbContext);

        // Act
        var result = await lotteryService.ListAsync();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task ListAsync_WithFilter_ReturnsFilteredLotteries()
    {
        // Arrange
        var dbContext = GetDbContextWithMockedData();
        var lotteryService = new Application.Services.Lotteries.LotteryService(dbContext);
        var filter = new LotteryFilter
        {
            From = new DateTime(2023, 1, 1),
            To = new DateTime(2023, 6, 30),
            ChannelId = 1
        };

        // Act
        var result = await lotteryService.ListAsync(filter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Lottery 1", result[0].Title);
    }
    
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