using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class SpamWordConfiguration : IEntityTypeConfiguration<SpamWord>
{
    public void Configure(EntityTypeBuilder<SpamWord> builder)
    {
        builder.ToTable("SpamWord");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.SpamWords)
            .HasForeignKey(e => e.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}