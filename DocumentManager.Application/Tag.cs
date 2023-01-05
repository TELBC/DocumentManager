using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class Tag
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public List<Document> Documents { get; set; }

    public Tag(string name, Category category)
    {
        Name = name;
        Category = category;
        Documents = new List<Document>();
    }
    protected Tag(){}
}