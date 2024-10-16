using Identity.API.Domain.Abstractions;

namespace Identity.API.Domain.Models
{
    public class User : Entity<Guid>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = [];
    }
}