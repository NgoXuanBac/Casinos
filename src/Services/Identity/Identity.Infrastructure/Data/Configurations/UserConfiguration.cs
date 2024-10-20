using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"tbl{nameof(User)}s").HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasConversion(n => n.Value, value => Email.Of(value))
                .HasMaxLength(Email.MAX_LENGTH)
                .HasColumnName(nameof(User.Email));

            builder.HasIndex(x => x.Email).IsUnique();

            builder.ComplexProperty(r => r.Password, builder =>
            {
                builder.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(Password.MAX_LENGTH)
                    .HasColumnName(nameof(User.Password));
            });
            builder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity("tblUserRoles");
        }
    }
}