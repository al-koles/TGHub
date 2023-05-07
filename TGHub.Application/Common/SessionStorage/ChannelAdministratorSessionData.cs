using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Application.Common.SessionStorage;

public class ChannelAdministratorSessionData : IMapWith<ChannelAdministrator>
{
    public int ChannelId { get; set; }
    public ChannelRole Role { get; set; }
}