using System.ComponentModel.DataAnnotations;

namespace DocumentManager;

public abstract class UserBase
{
    [Key]
    public int Id { get; protected set; }

    [Required]
    public string Name { get; protected set; }

    [Required]
    [EmailAddress]
    public string Email { get; protected set; }
}