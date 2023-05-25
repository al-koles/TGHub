using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class Lottery : EntityBase
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int WinnersCount { get; set; }
    public long? LotteryTelegramId { get; set; }
    public long? ResultTelegramId { get; set; }
    public Guid AttachmentsFolderId { get; set; } = Guid.NewGuid();
    public MediaGroupFormat AttachmentsFormat { get; set; } = MediaGroupFormat.PhotoVideo;

    public int CreatorId { get; set; }
    public ChannelAdministrator Creator { get; set; } = null!;

    public ICollection<LotteryAttachment> Attachments = new HashSet<LotteryAttachment>();
    public ICollection<LotteryParticipant> Participants = new HashSet<LotteryParticipant>();
}