using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class RoleConfiguration
        : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable($"tbl{nameof(Role)}s").HasKey(x => x.Name);
            builder.Property(x => x.Name)
                .HasConversion(n => n.Value, value => RoleName.Of(value))
                .HasMaxLength(RoleName.MAX_LENGTH)
                .HasColumnName(nameof(Role.Name));

            builder.ComplexProperty(r => r.Description, builder =>
            {
                builder.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Description.MAX_LENGTH)
                    .HasColumnName(nameof(Role.Description));
            });
            builder.HasMany(x => x.Permissions).WithMany(x => x.Roles).UsingEntity("tblRolePermissions");
        }
    }
}