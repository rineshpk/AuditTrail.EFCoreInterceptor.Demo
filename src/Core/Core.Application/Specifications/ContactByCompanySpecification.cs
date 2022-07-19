using Ardalis.Specification;
using Core.Domain.Entities.Application;

namespace Core.Application.Specifications;

public class ContactByCompanySpecification : Specification<Contact>
{
    public ContactByCompanySpecification(string company)
    {
        Query.Where(c => c.Company == company);
    }
}
