using Core.Application.Extensions;
using Core.Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using System.Reflection;

namespace Presentation.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        //services.AddMemoryCache();
        services.AddAutoMapper(typeof(Startup));
        services.AddHttpContextAccessor();
        services.AddInfrastructureLayer(Configuration);
        services.AddApplicationLayer();
        
        services.AddControllers();
        services.AddSwaggerGen();

        services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();

        services.AddMiniProfiler(options => //{
            options.RouteBasePath = "/profiler"
        //options.Storage = new SqlServerStorage(Configuration.GetConnectionString("ProfilerDBString"))
        //}
        ).AddEntityFramework();


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //Using a custom ui for swagger with mini profile ui injected. 
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Presentation.WebApi.Custom-SwaggerUI.html");
            c.RoutePrefix = string.Empty;
        });
        app.UseRouting();
        app.UseMiniProfiler();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
