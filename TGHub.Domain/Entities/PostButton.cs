namespace TGHub.Domain.Entities;

public class PostButton : EntityBase
{
    public string Content { get; set; } = null!;
    public string Link { get; set; } = null!;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}