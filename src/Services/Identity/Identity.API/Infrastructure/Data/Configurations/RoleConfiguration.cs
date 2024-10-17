using Identity.API.Domain.Models;
using Identity.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Infrastructure.Data.Configurations
{
    public class RoleConfiguration
        : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("tblRoles").HasKey(x => x.Name);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

            builder.ComplexProperty(r => r.Description, builder =>
            {
                builder.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Description.MaxLength);
            });
            builder.HasMany(x => x.Permissions).WithMany(x => x.Roles).UsingEntity("tblRolePermissions");
        }
    }
}