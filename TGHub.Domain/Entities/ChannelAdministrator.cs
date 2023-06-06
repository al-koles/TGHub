using TGHub.Domain.Enums;

namespace TGHub.Domain.Entities;

public class ChannelAdministrator : EntityBase
{
    public ChannelRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    public int AdministratorId;
    public TgHubUser Administrator { get; set; } = null!;

    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;

    public ICollection<Lottery> Lotteries = new HashSet<Lottery>();
    public ICollection<Post> Posts = new HashSet<Post>();
    public ICollection<ArchiveBann> InitiatedArchiveBanns = new HashSet<ArchiveBann>();
    public ICollection<ArchiveBann> InitiatedArchiveUnBanns = new HashSet<ArchiveBann>();
    public ICollection<BannUser> BannedUsers = new HashSet<BannUser>();
}