using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class BanWordConfiguration : IEntityTypeConfiguration<BannWord>
{
    public void Configure(EntityTypeBuilder<BannWord> builder)
    {
        builder.ToTable("BannWord");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.BannWords)
            .HasForeignKey(e => e.ChannelId);
    }
}