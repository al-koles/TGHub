using System.ComponentModel.DataAnnotations;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Posts.Models;

public class PostAttachmentModel : IMapWith<PostAttachment>
{
    public int Id { get; set; }

    [Required]
    public string Link { get; set; } = null!;
}