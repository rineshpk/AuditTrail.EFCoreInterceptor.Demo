using Core.Domain.Entities;
using Core.Domain.Entities.Application;

namespace Core.Application.Interfaces;

public interface IContactRepository : IRepository<Contact>
{
    Task<Contact> GetFullContact(int id);
    Task DeleteAsync(int id);
}
