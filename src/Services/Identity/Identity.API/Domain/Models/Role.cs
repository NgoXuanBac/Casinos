using System.Text.Json.Serialization;
using Identity.API.Domain.ValueObjects;

namespace Identity.API.Domain.Models
{
    public class Role
    {
        public required string Name { get; set; }
        public required Description Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Permission> Permissions { get; set; } = [];
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = [];
    }
}