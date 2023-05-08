using System.ComponentModel.DataAnnotations;
using TGHub.Application.Common.Mappings;
using TGHub.Domain.Entities;

namespace TGHub.Blazor.Pages.Posts.Models;

public class PostModel : IMapWith<Post>
{
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime ReleaseDateTime { get; set; }

    public long? TelegramId { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [Required] [ValidateComplexType]
    public List<PostButtonModel> Buttons { get; set; } = new();

    [Required]
    public List<PostAttachmentModel> Attachments { get; set; } = new();
}