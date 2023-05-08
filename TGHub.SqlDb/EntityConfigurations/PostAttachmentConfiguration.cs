using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class PostAttachmentConfiguration : IEntityTypeConfiguration<PostAttachment>
{
    public void Configure(EntityTypeBuilder<PostAttachment> builder)
    {
        builder.ToTable("PostAttachment");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Post)
            .WithMany(e => e.Attachments)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}