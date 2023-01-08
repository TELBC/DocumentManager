using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class Tag
{
    [Key]
    public int Id { get; protected set; }

    [Required]
    public string Name { get; protected set; }

    public Category Category { get; protected set; }

    [ForeignKey("DocumentId")]
    public ICollection<DocumentTag> DocumentTags { get; protected set; }

    protected Tag() { }
}