using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb;

public class TgHubDbContext : DbContext, ITgHubDbContext
{
    public TgHubDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BannUser> BannUsers { get; set; } = null!;
    public DbSet<ArchiveBann> Banns { get; set; } = null!;
    public DbSet<SpamMessage> SpamMessages { get; set; } = null!;
    public DbSet<SpamWord> SpamWords { get; set; } = null!;
    public DbSet<Channel> Channels { get; set; } = null!;
    public DbSet<ChannelAdministrator> ChannelAdministrators { get; set; } = null!;
    public DbSet<Lottery> Lotteries { get; set; } = null!;
    public DbSet<LotteryAttachment> LotteryAttachments { get; set; } = null!;
    public DbSet<LotteryParticipant> LotteryParticipants { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostAttachment> PostAttachments { get; set; } = null!;
    public DbSet<PostButton> PostButtons { get; set; } = null!;
    public DbSet<TgHubUser> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TgHubDbContext).Assembly);
    }
}