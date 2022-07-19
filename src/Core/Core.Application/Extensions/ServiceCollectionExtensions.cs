using Core.Application.Interfaces;
using Core.Application.Mappings;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        
        services.AddAutoMapper(config =>
        {
            config.AddProfile<ContactProfile>();
        });
        //services.AddMediator(x =>
        //{
        //    #region Commands

        //    #region User

        //    x.AddConsumer<CreateUserCommandHandler>();
        //    x.AddConsumer<ActivateUserCommandHandler>();
        //    x.AddConsumer<DeactivateUserCommandHandler>();
        //    x.AddConsumer<AddRolesCommandHandler>();
        //    x.AddConsumer<RemoveRolesCommandHandler>();
        //    x.AddConsumer<ChangeUserPasswordCommandHandler>();
        //    x.AddConsumer<UpdateUserRolesCommandHandler>();
        //    x.AddConsumer<UpdateUserDetailsCommandHandler>();
        //    x.AddConsumer<CreateNewOrderCommandHandler>();

        //    #endregion User

        //    #endregion Commands

        //    #region Queries

        //    x.AddConsumer<GetAllUsersQueryHandler>();
        //    x.AddConsumer<GetUserQueryHandler>();

        //    #endregion
        //});

        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
    }
}
