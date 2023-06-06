namespace TGHub.Domain.Entities;

public class SpamWord : EntityBase
{
    public string Value { get; set; } = null!;

    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
}