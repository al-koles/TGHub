using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Application.Common.SessionStorage;

public class ChannelAdministratorSessionData : IMapWith<ChannelAdministrator>
{
    public int ChannelId { get; set; }
    public int ChannelAdministratorId { get; set; }
    public ChannelRole Role { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChannelAdministrator, ChannelAdministratorSessionData>()
            .ForMember(dst => dst.ChannelAdministratorId,
                opt => opt.MapFrom(srs => srs.Id));
    }
}