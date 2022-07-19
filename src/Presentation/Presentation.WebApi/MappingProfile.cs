using AutoMapper;
using Core.Domain.Entities.Application;
using WebApi.DTOs;

namespace Presentation.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
