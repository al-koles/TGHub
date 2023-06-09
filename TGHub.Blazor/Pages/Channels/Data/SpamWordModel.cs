using System.ComponentModel.DataAnnotations;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Channels.Data;

public class SpamWordModel : IMapWith<SpamWord>
{
    [Required] public string Value { get; set; } = null!;

    [Required] public int ChannelId { get; set; }
}