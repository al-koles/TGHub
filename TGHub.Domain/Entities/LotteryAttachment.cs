namespace TGHub.Domain.Entities;

public class LotteryAttachment : EntityBase
{
    public string Link { get; set; } = null!;

    public int LotteryId { get; set; }
    public Lottery Lottery { get; set; } = null!;
}