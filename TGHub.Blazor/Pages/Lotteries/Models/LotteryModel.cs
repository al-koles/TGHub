using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Blazor.Data;
using TGHub.Blazor.Shared.Components.FileInput;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Blazor.Pages.Lotteries.Models;

public class LotteryModel : IMapWith<Lottery>
{
    public int Id { get; set; }

    [Required] public string Title { get; set; } = null!;

    [Required] public string Content { get; set; } = null!;

    [Required] public int WinnersCount { get; set; }

    [Required] public DateTimeOffset? StartDateTime { get; set; }

    [Required] public DateTimeOffset? EndDateTime { get; set; }

    public MediaGroupFormat AttachmentsFormat { get; set; } = MediaGroupFormat.PhotoVideo;

    public Guid AttachmentsFolderId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "The Channel field is required.")]
    public ChannelAdministrator Creator { get; set; }

    [Required] public List<CustomInputFileModel> Attachments { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<LotteryModel, Lottery>()
            .ForMember(dst => dst.Creator,
                opt => opt.Ignore())
            .ForMember(dst => dst.CreatorId,
                opt => opt.MapFrom(srs => srs.Creator.Id))
            .ForMember(dst => dst.StartDateTime,
                opt => opt.MapFrom(srs => srs.StartDateTime!.Value.DateTime))
            .ForMember(dst => dst.EndDateTime,
                opt => opt.MapFrom(srs => srs.EndDateTime!.Value.DateTime))
            .ForMember(dst => dst.Attachments,
                opt => opt.MapFrom(srs => srs.Attachments.Select(a => new LotteryAttachment
                {
                    FileName = a.File.Name,
                    Type = srs.AttachmentsFormat == MediaGroupFormat.PhotoVideo
                        ? AttachmentFormatsHelper.GetInAlbumType(Path.GetExtension(a.File.Name))
                        : srs.AttachmentsFormat == MediaGroupFormat.Audio
                            ? AttachmentType.Audio
                            : AttachmentType.Document
                }).ToList()));

        profile.CreateMap<Lottery, LotteryModel>()
            .ForMember(dst => dst.Attachments,
                opt => opt.Ignore());
    }
}