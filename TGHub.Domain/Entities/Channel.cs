namespace TGHub.Domain.Entities;

public class Channel : EntityBase
{
    public long TelegramId { get; set; }
    public long? LinkedChatTelegramId { get; set; }
    public string Name { get; set; } = null!;
    public bool OffensiveSpamOn { get; set; }
    public bool ListSpamOn { get; set; }
    public bool IsActive { get; set; }
    public string? LogoFileName { get; set; }

    public ICollection<ChannelAdministrator> Administrators { get; set; } = new List<ChannelAdministrator>();
    public ICollection<SpamWord> SpamWords { get; set; } = new HashSet<SpamWord>();
    public ICollection<Spammer> Spammers { get; set; } = new HashSet<Spammer>();
}