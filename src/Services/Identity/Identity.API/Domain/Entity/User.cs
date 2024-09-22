using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Identity.API.Common;

namespace Identity.API.Domain.Entity;

[Table("tblUsers")]
public class User : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; } = [];
}