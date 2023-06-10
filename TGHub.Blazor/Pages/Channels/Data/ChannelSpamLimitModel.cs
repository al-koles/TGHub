using System.ComponentModel.DataAnnotations;

namespace TGHub.Blazor.Pages.Channels.Data;

public class ChannelSpamLimitModel
{
    [Required] [Range(0, int.MaxValue)] public int ChannelSpamLimit { get; set; }

    [Required] public int ChannelId { get; set; }
}