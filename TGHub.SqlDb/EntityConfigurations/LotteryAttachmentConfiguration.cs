using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class LotteryAttachmentConfiguration : IEntityTypeConfiguration<LotteryAttachment>
{
    public void Configure(EntityTypeBuilder<LotteryAttachment> builder)
    {
        builder.ToTable("LotteryAttachment");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Lottery)
            .WithMany(e => e.Attachments)
            .HasForeignKey(e => e.LotteryId);
    }
}