using System.Text.Json.Serialization;

namespace Identity.API.Domain.Models
{
    public class Permission
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Path { get; set; }
        public required string Method { get; set; }
        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; } = [];
    }
}