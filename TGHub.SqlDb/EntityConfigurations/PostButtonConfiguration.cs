using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class PostButtonConfiguration : IEntityTypeConfiguration<PostButton>
{
    public void Configure(EntityTypeBuilder<PostButton> builder)
    {
        builder.ToTable("PostButton");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Post)
            .WithMany(e => e.Buttons)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}