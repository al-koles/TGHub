using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class SpamMessageConfiguration : IEntityTypeConfiguration<SpamMessage>
{
    public void Configure(EntityTypeBuilder<SpamMessage> builder)
    {
        builder.ToTable("SpamMessage");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.SpammerId, e.TelegramId }).IsUnique();

        builder.HasOne(e => e.Spammer)
            .WithMany(e => e.SpamMessages)
            .HasForeignKey(e => e.SpammerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}