namespace TGHub.Domain.Entities;

public class ArchiveBann : EntityBase
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? BannContext { get; set; }
    public string? UnBannContext { get; set; }

    public int SpammerId { get; set; }
    public Spammer Spammer { get; set; } = null!;

    public int? InitiatorId { get; set; }
    public ChannelAdministrator? Initiator { get; set; }

    public int? UnBannInitiatorId { get; set; }
    public ChannelAdministrator? UnBannInitiator { get; set; }
}