using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Tag
{
    [Key]
    public int Id { get; protected set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; protected set; }

    public Category Category { get; protected set; }

    #pragma warning disable CS8618
    protected Tag() { }
    #pragma warning restore CS8618
}

//updated to what is required in the project