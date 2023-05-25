using TGHub.Domain.Enums;

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
    public string ResultMessage { get; set; } = null!;
    public Guid AttachmentsFolderId { get; set; } = Guid.NewGuid();
    public MediaGroupFormat AttachmentsFormat { get; set; } = MediaGroupFormat.PhotoVideo;
    public string VoteButtonContent { get; set; } = null!;

    public int CreatorId { get; set; }
    public ChannelAdministrator Creator { get; set; } = null!;

    public ICollection<LotteryAttachment> Attachments = new HashSet<LotteryAttachment>();
    public ICollection<LotteryParticipant> Participants = new HashSet<LotteryParticipant>();
}