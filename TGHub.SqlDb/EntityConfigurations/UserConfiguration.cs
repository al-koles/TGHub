using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<TgHubUser>
{
    public void Configure(EntityTypeBuilder<TgHubUser> builder)
    {
        builder.ToTable("User");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.TelegramId).IsUnique();
    }
}