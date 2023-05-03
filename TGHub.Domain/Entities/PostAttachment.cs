namespace TGHub.Domain.Entities;

public class PostAttachment : EntityBase
{
    public string Link { get; set; } = null!;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}