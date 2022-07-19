using Core.Domain.Entities.Application;

namespace Core.Application.Interfaces;

public interface IContactService
{
    Task<List<Contact>> GetAll();

    Task<Contact> Add(Contact contact);

    Task<Contact> Update(Contact contact);

    Task Delete(int id);

    Task<List<Contact>> GetContactByCompany(string company);

    Task<Contact> GetFullContact(int id);

}
