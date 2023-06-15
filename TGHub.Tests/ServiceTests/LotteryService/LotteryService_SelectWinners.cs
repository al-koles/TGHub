using TGHub.Domain.Entities;

namespace TGHub.Tests.ServiceTests.LotteryService;

public class LotteryService_SelectWinners
{
    [Fact]
    public void SelectWinners_ParticipantCountLessThanOrEqualWinnersCount_AllParticipantsAreWinners()
    {
        // Arrange
        var lotteryService = new Application.Services.Lotteries.LotteryService(null);
        var lottery = new Lottery
        {
            WinnersCount = 5,
            Participants = new List<LotteryParticipant>
            {
                new(),
                new(),
                new(),
                new(),
                new()
            }
        };

        // Act
        lotteryService.SelectWinners(lottery);

        // Assert
        Assert.True(lottery.Participants.All(p => p.IsWinner));
    }

    [Fact]
    public void SelectWinners_ParticipantCountGreaterThanWinnersCount_SelectsRandomWinners()
    {
        // Arrange
        var lotteryService = new Application.Services.Lotteries.LotteryService(null);
        var lottery = new Lottery
        {
            WinnersCount = 3,
            Participants = new List<LotteryParticipant>
            {
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new()
            }
        };

        // Act
        lotteryService.SelectWinners(lottery);

        // Assert
        Assert.Equal(3, lottery.Participants.Count(p => p.IsWinner));
    }
}