using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Folder : IEntity<int>
{
    public Folder(string name, List<Document>? documents)
    {
        Name = name;
        Documents = documents;
    }
    #pragma warning disable CS8618
    public Folder() { }
    #pragma warning restore CS8618
    
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    public List<Document> Documents { get; set; }
}
//updated to what is required in the project