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

        builder.HasIndex(e => new { e.ChannelId, e.AuthorTelegramId });

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.SpamMessages)
            .HasForeignKey(e => e.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}