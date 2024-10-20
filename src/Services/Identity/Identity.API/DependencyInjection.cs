using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Models.Response.Filter;

namespace Identity.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddApiVersioning(options => { options.ReportApiVersions = true; })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        services.AddAuthentication(configuration);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseExceptionHandler(options => { });
        app.UseAuthentication();
        app.UseCustomAuthorization();

        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet)
            .AddEndpointFilter<CustomResponseFilter>()
            .MapCarter();

        return app;
    }
}
