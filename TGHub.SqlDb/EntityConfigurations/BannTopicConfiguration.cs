using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class BannTopicConfiguration : IEntityTypeConfiguration<BannTopic>
{
    public void Configure(EntityTypeBuilder<BannTopic> builder)
    {
        builder.ToTable("BannTopic");

        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Channels)
            .WithMany(e => e.BannTopics)
            .UsingEntity("ChannelBannTopic");
    }
}