using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Shared.Components.Calendar.Models;

public class CalendarEventModel : IMapWith<Lottery>
{
    public int Id { get; set; }
    public long? TelegramId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime ReleaseDateTime { get; set; }
    public CalendarEventStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Lottery, CalendarEventModel>().ReverseMap();
        profile.CreateMap<Post, CalendarEventModel>().ReverseMap();
    }
}