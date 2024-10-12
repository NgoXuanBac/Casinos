using System.Text.Json.Serialization;
using Identity.Domain.Abstractions;

namespace Identity.Domain.Models;

public class Role
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Permission> Permissions { get; set; } = [];
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = [];
}