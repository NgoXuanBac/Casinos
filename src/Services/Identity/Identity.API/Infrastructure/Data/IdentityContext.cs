using System.Reflection;
using Identity.API.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Data;

public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}