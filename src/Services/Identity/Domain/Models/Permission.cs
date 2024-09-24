using System.Text.Json.Serialization;
using Identity.API.Domain.Abstractions;

namespace Identity.API.Domain.Models;

public class Permission : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Url { get; set; }
    public required string Method { get; set; }
    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; } = [];
}