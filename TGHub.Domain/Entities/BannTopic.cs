namespace TGHub.Domain.Entities;

public class BannTopic : EntityBase
{
    public string Value { get; set; } = null!;

    public ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();
}