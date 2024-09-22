using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Identity.API.Common;

namespace Identity.API.Domain.Entity;

[Table("tblRoles")]
public class Role : AuditableEntity
{
    [Key]
    public required string Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Permission> Permissions { get; set; } = [];
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = [];
}