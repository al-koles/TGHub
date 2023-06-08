using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class SpamMessage : EntityBase
{
    public int TelegramId { get; set; }
    public long AuthorTelegramId { get; set; }
    public string Value { get; set; } = null!;
    public DateTime DateTimeWritten { get; set; }
    public SpamMessageType Type { get; set; }
    public string Context { get; set; } = null!;

    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
}