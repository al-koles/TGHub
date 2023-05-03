namespace TGHub.Domain.Entities;

public class Post : EntityBase
{
    public string Content { get; set; } = null!;
    public DateTime ReleaseDateTime { get; set; }
    public string? TelegramId { get; set; }

    public int CreatorId { get; set; }
    public ChannelAdministrator Creator { get; set; } = null!;

    public ICollection<PostButton> Buttons { get; set; } = new HashSet<PostButton>();
    public ICollection<PostAttachment> Attachments { get; set; } = new HashSet<PostAttachment>();
}