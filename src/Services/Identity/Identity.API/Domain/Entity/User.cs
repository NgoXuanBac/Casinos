using Identity.API.Common;
namespace Identity.API.Domain.Entity;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public virtual ICollection<Role> Roles { get; set; } = [];
}