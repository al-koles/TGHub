using TGHub.Application.Services.Lotteries.Data;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries.Interfaces;

public interface ILotteryScheduleService
{
    Task ScheduleLotteryAsync(Lottery lottery);
    Task ScheduleResultAsync(Lottery lottery);
    Task UnscheduleLotteryAsync(Lottery lottery);
    Task UnscheduleResultAsync(Lottery lottery);
    Task<LotterySendStatus> GetLotterySendStatusAsync(Lottery lottery);
    Task<LotterySendStatus> GetResultSendStatusAsync(Lottery lottery);
}