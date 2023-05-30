using TGHub.Domain.Entities;

namespace TGHub.Telegram.Bot.Lotteries;

public interface ITgLotteryService
{
    Task<int> SendLotteryAsync(Lottery lottery);
    Task<int> SendResultAsync(Lottery lottery);
}