using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddDatabaseAuditing(configuration);
        services.AddRepositories();
       
        services.AddAutoMapper(config =>
        {
            config.AddProfile<AppProfile>();
        });
    }

    private static void AddDatabasePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        //{
        //    services.AddDbContext<ApplicationDbContext>(options =>
        //        options.UseInMemoryDatabase("ApplicationDb"));
        //}
        //else
        //{
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //}
    }

    private static void AddDatabaseAuditing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuditDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ISaveChangesInterceptor, AuditingInterceptor>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IContactRepository, ContactRepository>();
    }

    

}
