using Ardalis.Specification;

namespace Core.Application.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
