using Ardalis.Specification.EntityFrameworkCore;
using Core.Application.Interfaces;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
