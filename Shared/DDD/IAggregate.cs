namespace Shared.DDD
{
    public interface IAggregate<TId> : IEntity<TId>, IAggregate
    {
    }
    public interface IAggregate : IEntity
    {
         IReadOnlyList<IDomainEvent> DomainEvents { get; }

        IDomainEvent[]  ClearDomainEvents();

    }
}
