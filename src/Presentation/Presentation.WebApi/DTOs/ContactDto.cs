namespace WebApi.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        public virtual List<AddressDto> Addresses { get; } = new List<AddressDto>();
    }
}
