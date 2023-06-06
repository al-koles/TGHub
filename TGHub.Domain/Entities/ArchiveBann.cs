namespace TGHub.Domain.Entities;

public class ArchiveBann : EntityBase
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Context { get; set; }

    public int UserId { get; set; }
    public BannUser User { get; set; } = null!;
    
    public int? InitiatorId { get; set; }
    public ChannelAdministrator? Initiator { get; set; }
    
    public int? UnBannInitiatorId { get; set; }
    public ChannelAdministrator? UnBannInitiator { get; set; }
}