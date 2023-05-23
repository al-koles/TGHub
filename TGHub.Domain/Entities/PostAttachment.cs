using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class PostAttachment : EntityBase
{
    public string FileName { get; set; } = null!;
    public AttachmentType Type { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}