using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Channels;

public class MappingProfile : IMapWith<object>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Channel, Channel>();
    }
}