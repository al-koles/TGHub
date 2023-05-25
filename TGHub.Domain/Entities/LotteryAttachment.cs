using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class LotteryAttachment : EntityBase
{
    public string FileName { get; set; } = null!;
    public AttachmentType Type { get; set; }

    public int LotteryId { get; set; }
    public Lottery Lottery { get; set; } = null!;
}