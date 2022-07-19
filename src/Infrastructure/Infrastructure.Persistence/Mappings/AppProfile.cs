using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Entities.Application;

namespace Infrastructure.Persistence.Mappings;

internal class AppProfile : Profile
{
    public AppProfile()
    {
        //CreateMap<Core.Domain.Entities.Contact, ContactEntity>().ReverseMap();
        //CreateMap<Address, AddressEntity>().ReverseMap();
        //CreateMap<Core.Domain.Entities.State, StateEntity>().ReverseMap();
    }
}
