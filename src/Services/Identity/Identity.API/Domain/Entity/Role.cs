using System.Text.Json.Serialization;
using Identity.API.Common;

namespace Identity.API.Domain.Entity;

public class Role : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Permission> Permissions { get; set; } = [];
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = [];
}