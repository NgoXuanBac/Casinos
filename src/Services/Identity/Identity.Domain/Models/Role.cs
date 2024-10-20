using System.Text.Json.Serialization;

namespace Identity.Domain.Models
{
    public class Role
    {
        public required RoleName Name { get; set; }
        public required Description Description { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; } = [];
        public virtual ICollection<User> Users { get; set; } = [];
    }
}