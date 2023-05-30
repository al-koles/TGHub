using TGHub.Application.Common.Filtering;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries.Data;

public class LotteryFilter : FilterBase<Lottery>
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? ChannelId { get; set; }
    public int? ChannelAdministratorId { get; set; }
}