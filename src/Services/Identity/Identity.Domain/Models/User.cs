namespace Identity.Domain.Models
{
    public class User : Entity<Guid>
    {
        public required Email Email { get; set; }
        public required Password Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = [];
    }
}