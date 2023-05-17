﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TGHub.Application.Common.Mappings;
using TGHub.Blazor.Data;
using TGHub.Blazor.Shared.Components.FileInput;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Posts.Models;

public class PostModel : IMapWith<Post>
{
    private TimeOnly? _releaseTime;
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public DateOnly? ReleaseDate { get; set; }

    [Required]
    public string? ReleaseTime
    {
        get => _releaseTime?.ToString("HH:mm");
        set => _releaseTime = TimeOnly.TryParse(value ?? "", out var time) ? time : null;
    }

    public long? TelegramId { get; set; }

    public Guid AttachmentsFolderId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "The Channel field is required.")]
    public ChannelAdministrator Creator { get; set; }

    [Required] [ValidateComplexType]
    public List<PostButtonModel> Buttons { get; set; } = new();

    [Required]
    public List<CustomInputFileModel> Attachments { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PostModel, Post>()
            .ForMember(dst => dst.Creator,
                opt => opt.Ignore())
            .ForMember(dst => dst.CreatorId,
                opt => opt.MapFrom(srs => srs.Creator.Id))
            .ForMember(dst => dst.ReleaseDateTime,
                opt => opt.MapFrom(srs => srs.ReleaseDate!.Value.ToDateTime(srs._releaseTime!.Value)))
            .ForMember(dst => dst.Attachments,
                opt => opt.MapFrom(srs => srs.Attachments.Select(a => new PostAttachment
                {
                    FileName = a.File.Name,
                    Type = AttachmentFormatsHelper.GetAttachmentType(Path.GetExtension(a.File.Name))
                }).ToList()));

        profile.CreateMap<Post, PostModel>()
            .ForMember(dst => dst.ReleaseDate,
                opt => opt.MapFrom(srs => DateOnly.FromDateTime(srs.ReleaseDateTime)))
            .ForMember(dst => dst.ReleaseTime,
                opt => opt.MapFrom(srs => TimeOnly.FromDateTime(srs.ReleaseDateTime)));
    }
}