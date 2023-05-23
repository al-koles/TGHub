using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class Post : EntityBase
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime ReleaseDateTime { get; set; }
    public int? TelegramId { get; set; }
    public Guid AttachmentsFolderId { get; set; } = Guid.NewGuid();

    public MediaGroupFormat AttachmentsFormat { get; set; } = MediaGroupFormat.PhotoVideo;

    public int CreatorId { get; set; }
    public ChannelAdministrator Creator { get; set; } = null!;

    public ICollection<PostButton> Buttons { get; set; } = new HashSet<PostButton>();
    public ICollection<PostAttachment> Attachments { get; set; } = new HashSet<PostAttachment>();
}