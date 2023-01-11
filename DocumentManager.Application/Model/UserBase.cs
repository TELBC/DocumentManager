using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public abstract class UserBase : IEntity<int>
{
    [Key]
    public int Id { get; protected set; }

    [Required]
    public string Name { get; protected set; }

    [Required]
    [EmailAddress]
    public string Email { get; protected set; }
}