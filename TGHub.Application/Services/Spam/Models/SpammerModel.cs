using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spam.Models;

public class SpammerModel : IMapWith<Spammer>
{
    public int ChannelId { get; set; }
    public long TelegramId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
}