namespace TGHub.Domain.Entities;

public class TgHubUser : EntityBase
{
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? PhotoUrl { get; set; }

    public ICollection<ChannelAdministrator> AdministratedChannels { get; set; } = new List<ChannelAdministrator>();
}