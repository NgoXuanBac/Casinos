using Identity.API.Domain.Models;
using Identity.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Identity.API.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration
        : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("tblPermissions").HasKey(x => x.Name);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.ComplexProperty(r => r.Description, builder =>
            {
                builder.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Description.MaxLength);
            });

            builder.ComplexProperty(p => p.Endpoint, builder =>
            {
                builder.Property(e => e.Path)
                   .IsRequired()
                   .HasMaxLength(ApiEndpoint.MaxPathLength);

                builder.Property(e => e.Method)
                   .IsRequired();
            });
        }
    }
}