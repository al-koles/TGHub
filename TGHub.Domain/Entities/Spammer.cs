namespace TGHub.Domain.Entities;

public class Spammer : EntityBase
{
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public DateTime? BannDateTime { get; set; }
    public string? BannContext { get; set; }

    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
    
    public int? BannInitiatorId { get; set; }
    public ChannelAdministrator? BannInitiator { get; set; }

    public ICollection<ArchiveBann> ArchiveBanns = new HashSet<ArchiveBann>();
    public ICollection<SpamMessage> SpamMessages = new HashSet<SpamMessage>();
}