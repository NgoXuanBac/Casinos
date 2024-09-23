using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Identity.API.Common;

namespace Identity.API.Domain.Entity;

public class Permission : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Url { get; set; }
    public required string Method { get; set; }
    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; } = [];
}