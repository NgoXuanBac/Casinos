using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Identity.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration
        : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable($"tbl{nameof(Permission)}s").HasKey(x => x.Name);

            builder.Property(x => x.Name)
                .HasConversion(n => n.Value, value => PermissionName.Of(value))
                .HasMaxLength(PermissionName.MAX_LENGTH)
                .HasColumnName(nameof(Permission.Name));

            builder.ComplexProperty(r => r.Description, builder =>
            {
                builder.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Description.MAX_LENGTH)
                    .HasColumnName(nameof(Permission.Description));
            });

            builder.ComplexProperty(p => p.Endpoint, builder =>
            {
                builder.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(ApiEndpoint.MAX_PATH_LENGTH)
                    .HasColumnName(nameof(ApiEndpoint.Path));

                builder.Property(e => e.Method)
                    .IsRequired()
                    .HasColumnName(nameof(ApiEndpoint.Method));
            });
        }
    }
}