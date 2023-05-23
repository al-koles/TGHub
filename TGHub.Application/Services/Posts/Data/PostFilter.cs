using TGHub.Application.Common.Filtering;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts.Data;

public class PostFilter : FilterBase<Post>
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? ChannelId { get; set; }
    public int? ChannelAdministratorId { get; set; }
}