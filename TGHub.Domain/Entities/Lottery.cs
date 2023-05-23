namespace TGHub.Domain.Entities;

public class Lottery : EntityBase
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public int WinnersCount { get; set; }
    public long? LotteryTelegramId { get; set; }
    public long? ResultTelegramId { get; set; }

    public int CreatorId { get; set; }
    public ChannelAdministrator Creator { get; set; } = null!;

    public ICollection<LotteryAttachment> Attachments = new HashSet<LotteryAttachment>();
    public ICollection<LotteryParticipant> Participants = new HashSet<LotteryParticipant>();
}