namespace TGHub.Domain.Entities;

public class BannedUser : EntityBase
{
    public string TelegramId { get; set; } = null!;
    public string Context { get; set; } = null!;
    public DateTime BannDate { get; set; }
    public DateTime? BannTo { get; set; }

    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
}