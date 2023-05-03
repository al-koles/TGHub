using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.User;

public class UserService : Service<TgHubUser>, IUserService
{
    public UserService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }
}