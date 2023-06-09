using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.ArchiveBanns;

public class ArchiveBannService : Service<ArchiveBann>, IArchiveBannService
{
    public ArchiveBannService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }
}