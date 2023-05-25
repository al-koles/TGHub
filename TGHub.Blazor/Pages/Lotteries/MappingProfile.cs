using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Application.Services.Lotteries.Data;
using TGHub.Blazor.Data;
using TGHub.Blazor.Shared.Components.AttachmentTile;
using TGHub.Blazor.Shared.Components.Calendar.Models;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Lotteries;

public class MappingProfile : IMapWith<object>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<LotterySendStatus, CalendarEventStatus>().ConvertUsing((srs, _) => srs switch
        {
            LotterySendStatus.Sent => CalendarEventStatus.Succeed,
            LotterySendStatus.FailedToSend => CalendarEventStatus.Error,
            LotterySendStatus.Scheduled => CalendarEventStatus.Pending,
            LotterySendStatus.NotScheduled => CalendarEventStatus.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(srs), srs, null)
        });
        profile.CreateMap<LotteryAttachment, AttachmentTileModel>()
            .ForMember(dst => dst.Url,
                opt => opt.MapFrom((_, _, _, context) => context.Items[nameof(AttachmentTileModel.Url)]))
            .ForMember(dst => dst.Format,
                opt => opt.MapFrom(srs => AttachmentFormatsHelper.GetType(Path.GetExtension(srs.FileName))));
    }
}