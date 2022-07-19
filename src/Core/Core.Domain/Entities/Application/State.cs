namespace Core.Domain.Entities.Application;

public partial class State : DataEntityBase<int>
{
    public string StateName { get; set; } = null!;

    public virtual List<Address> Addresses { get; set; }
}
