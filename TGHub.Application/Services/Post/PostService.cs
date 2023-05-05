using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;

namespace TGHub.Application.Services.Post;

public class PostService : Service<Domain.Entities.Post>, IPostService
{
    protected PostService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }
}