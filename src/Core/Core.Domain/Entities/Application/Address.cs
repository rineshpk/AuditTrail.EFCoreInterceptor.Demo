using System.Text.Json.Serialization;

namespace Core.Domain.Entities.Application;

public partial class Address : DataEntityBase<int>
{
    public int ContactId { get; set; }
    public string AddressType { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public int StateId { get; set; }
    public string PostalCode { get; set; }

    [JsonIgnore]
    public virtual Contact Contact { get; } = null!;
    public virtual State State { get; } = null!;
}
