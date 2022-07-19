namespace Core.Domain.Entities.Application;

public partial class Contact : DataEntityBase<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
    public string Title { get; set; }

    public virtual List<Address> Addresses { get; } = new List<Address>();

}
