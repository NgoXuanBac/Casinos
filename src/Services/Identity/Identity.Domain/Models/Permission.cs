using System.Text.Json.Serialization;

namespace Identity.Domain.Models
{
    public class Permission
    {
        public required PermissionName Name { get; set; }
        public required Description Description { get; set; }
        public required ApiEndpoint Endpoint { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = [];
    }
}