using Identity.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tblUsers").HasKey(x => x.Id);
        builder.Property(x => x.Email).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Password).HasMaxLength(20).IsRequired();
        builder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity("tblUserRoles");
    }
}