using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class BannedUserConfiguration : IEntityTypeConfiguration<BannedUser>
{
    public void Configure(EntityTypeBuilder<BannedUser> builder)
    {
        builder.ToTable("BannedUser");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.ChannelId, e.TelegramId }).IsUnique();

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.BannedUsers)
            .HasForeignKey(e => e.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}