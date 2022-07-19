using Core.Application.Interfaces;
using Core.Application.Specifications;
using Core.Domain.Entities.Application;

namespace Core.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="contactRepository"></param>
    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<List<Contact>> GetAll()
    {
        return await _contactRepository.ListAsync();
    }

    public async Task<Contact> Add(Contact contact)
    {
        return await _contactRepository.AddAsync(contact);
    }

    public async Task<Contact> Update(Contact contact)
    {
        await _contactRepository.UpdateAsync(contact);
        return contact;
    }


    public async Task Delete(int id)
    {
        await _contactRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Specification example
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    public async Task<List<Contact>> GetContactByCompany(string company)
    {
        return await _contactRepository.ListAsync(new ContactByCompanySpecification(company));
    }

    public async Task<Contact> GetFullContact(int id)
    {
        return await _contactRepository.GetFullContact(id);
    }
}
