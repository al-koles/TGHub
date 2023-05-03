namespace TGHub.Domain.Entities;

public class LotteryParticipant : EntityBase
{
    public string TelegramId { get; set; } = null!;
    public bool IsWinner { get; set; }

    public int LotteryId { get; set; }
    public Lottery Lottery { get; set; } = null!;
}