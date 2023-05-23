using TGHub.Domain.Enums;

namespace TGHub.Blazor.Shared.Components.AttachmentTile;

public class AttachmentTileModel
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public AttachmentType Format { get; set; }
}