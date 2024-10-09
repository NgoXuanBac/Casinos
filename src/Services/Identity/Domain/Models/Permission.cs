using System.Text.Json.Serialization;
using Identity.Domain.Abstractions;

namespace Identity.Domain.Models;

public class Permission : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Path { get; set; }
    public required string Method { get; set; }
    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; } = [];
}