using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class ArchiveBannConfiguration : IEntityTypeConfiguration<ArchiveBann>
{
    public void Configure(EntityTypeBuilder<ArchiveBann> builder)
    {
        builder.ToTable("ArchiveBann");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.ArchiveBanns)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Initiator)
            .WithMany(e => e.InitiatedArchiveBanns)
            .HasForeignKey(e => e.InitiatorId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(e => e.UnBannInitiator)
            .WithMany(e => e.InitiatedArchiveUnBanns)
            .HasForeignKey(e => e.UnBannInitiatorId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}