using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class ChannelAdministratorConfiguration : IEntityTypeConfiguration<ChannelAdministrator>
{
    public void Configure(EntityTypeBuilder<ChannelAdministrator> builder)
    {
        builder.ToTable("ChannelAdministrator");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.ChannelId, e.AdministratorId }).IsUnique();

        builder.HasOne(e => e.Administrator)
            .WithMany(e => e.AdministratedChannels)
            .HasForeignKey(e => e.AdministratorId);

        builder.HasOne(e => e.Channel)
            .WithMany(e => e.Administrators)
            .HasForeignKey(e => e.ChannelId);
    }
}