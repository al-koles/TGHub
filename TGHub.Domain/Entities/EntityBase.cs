using TGHub.Domain.Interfaces;

namespace TGHub.Domain.Entities;

public abstract class EntityBase : IEntity
{
    public int Id { get; set; }
}