namespace Core.Domain.Interfaces;

/// <summary>
/// Generic abstraction for a domain entity
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEntity<TId>
{
    TId Id { get; }
}
