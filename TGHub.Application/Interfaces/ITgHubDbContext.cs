using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TGHub.Domain.Entities;

namespace TGHub.Application.Interfaces;

public interface ITgHubDbContext
{
    DbSet<BannedUser> BannedUsers { get; set; }
    DbSet<BannTopic> BannTopics { get; set; }
    DbSet<BannWord> BannWords { get; set; }
    DbSet<Channel> Channels { get; set; }
    DbSet<ChannelAdministrator> ChannelAdministrators { get; set; }
    DbSet<Lottery> Lotteries { get; set; }
    DbSet<LotteryAttachment> LotteryAttachments { get; set; }
    DbSet<LotteryParticipant> LotteryParticipants { get; set; }
    DbSet<Post> Posts { get; set; }
    DbSet<PostAttachment> PostAttachments { get; set; }
    DbSet<PostButton> PostButtons { get; set; }
    DbSet<TgHubUser> Users { get; set; }
    ChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}