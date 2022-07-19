namespace Core.Domain.Interfaces
{
    /// <summary>
    /// Interface for aggregate root
    /// </summary>
    public interface IAggregateRoot<TId> : IEntity<TId>
    {
        int Version { get; }

        bool IsDeleted { get; }
    }
}