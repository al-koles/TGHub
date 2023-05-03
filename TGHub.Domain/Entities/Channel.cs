namespace TGHub.Domain.Entities;

public class Channel : EntityBase
{
    public string TelegramId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool SpamOn { get; set; }

    public ICollection<ChannelAdministrator> Administrators { get; set; } = new List<ChannelAdministrator>();
    public ICollection<BannTopic> BannTopics { get; set; } = new HashSet<BannTopic>();
    public ICollection<BannWord> BannWords { get; set; } = new HashSet<BannWord>();
    public ICollection<BannedUser> BannedUsers { get; set; } = new HashSet<BannedUser>();
}