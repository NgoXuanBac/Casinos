using System.Text.Json.Serialization;
using Identity.API.Domain.ValueObjects;

namespace Identity.API.Domain.Models
{
    public class Permission
    {
        public required string Name { get; set; }
        public required Description Description { get; set; }
        public required ApiEndpoint Endpoint { get; set; }
        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; } = [];
    }
}