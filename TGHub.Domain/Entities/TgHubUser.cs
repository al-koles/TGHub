namespace TGHub.Domain.Entities;

public class TgHubUser : EntityBase
{
    public string TelegramId { get; set; } = null!;
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public string? PhotoUrl { get; set; } = null!;

    public ICollection<ChannelAdministrator> AdministratedChannels { get; set; } = new List<ChannelAdministrator>();
}