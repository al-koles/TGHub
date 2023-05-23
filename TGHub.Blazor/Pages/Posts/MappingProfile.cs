using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Application.Services.Posts.Data;
using TGHub.Blazor.Shared.Components.Calendar.Models;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Posts;

public class MappingProfile : IMapWith<object>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<PostSendStatus, CalendarEventStatus>().ConvertUsing((srs, _) => srs switch
        {
            PostSendStatus.Sent => CalendarEventStatus.Succeed,
            PostSendStatus.FailedToSend => CalendarEventStatus.Error,
            PostSendStatus.Scheduled => CalendarEventStatus.Pending,
            PostSendStatus.NotScheduled => CalendarEventStatus.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(srs), srs, null)
        });
        profile.CreateMap<Lottery, CalendarEventModel>().ReverseMap();
        profile.CreateMap<Post, CalendarEventModel>().ReverseMap();
    }
}