using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class LotteryParticipantConfiguration : IEntityTypeConfiguration<LotteryParticipant>
{
    public void Configure(EntityTypeBuilder<LotteryParticipant> builder)
    {
        builder.ToTable("LotteryParticipant");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.LotteryId, e.TelegramId });

        builder.HasOne(e => e.Lottery)
            .WithMany(e => e.Participants)
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}