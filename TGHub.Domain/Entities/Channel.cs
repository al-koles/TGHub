namespace TGHub.Domain.Entities;

public class Channel : EntityBase
{
    public long TelegramId { get; set; }
    public long? LinkedChatTelegramId { get; set; }
    public string Name { get; set; } = null!;
    public bool SpamOn { get; set; }
    public bool IsActive { get; set; }
    public string? LogoFileName { get; set; }

    public ICollection<ChannelAdministrator> Administrators { get; set; } = new List<ChannelAdministrator>();
    public ICollection<BannTopic> BannTopics { get; set; } = new HashSet<BannTopic>();
    public ICollection<BannWord> BannWords { get; set; } = new HashSet<BannWord>();
    public ICollection<BannedUser> BannedUsers { get; set; } = new HashSet<BannedUser>();
}