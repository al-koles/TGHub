using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TGHub.Domain.Entities;

namespace TGHub.Application.Interfaces;

public interface ITgHubDbContext : IDisposable, IAsyncDisposable
{
    DbSet<BannUser> BannUsers { get; set; }
    DbSet<ArchiveBann> Banns { get; set; }
    DbSet<SpamMessage> SpamMessages { get; set; }
    DbSet<SpamWord> SpamWords { get; set; }
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