using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Data;

public static class Extentions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
        return app;
    }
}