using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.SqlDb;

public class TgHubDbContext : DbContext, ITgHubDbContext
{
    public TgHubDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BannedUser> BannedUsers { get; set; } = null!;
    public DbSet<BannTopic> BannTopics { get; set; } = null!;
    public DbSet<BannWord> BannWords { get; set; } = null!;
    public DbSet<Channel> Channels { get; set; } = null!;
    public DbSet<ChannelAdministrator> ChannelAdministrators { get; set; } = null!;
    public DbSet<Lottery> Lotteries { get; set; } = null!;
    public DbSet<LotteryAttachment> LotteryAttachments { get; set; } = null!;
    public DbSet<LotteryParticipant> LotteryParticipants { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostAttachment> PostAttachments { get; set; } = null!;
    public DbSet<PostButton> PostButtons { get; set; } = null!;
    public DbSet<TgHubUser> Users { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TgHubDbContext).Assembly);
    }
}