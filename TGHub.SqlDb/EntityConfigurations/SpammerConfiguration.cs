using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class SpammerConfiguration : IEntityTypeConfiguration<Spammer>
{
    public void Configure(EntityTypeBuilder<Spammer> builder)
    {
        builder.ToTable("Spammer");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.ChannelId, e.TelegramId }).IsUnique();

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.Spammers)
            .HasForeignKey(e => e.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.BannInitiator)
            .WithMany(e => e.BannedUsers)
            .HasForeignKey(e => e.BannInitiatorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}