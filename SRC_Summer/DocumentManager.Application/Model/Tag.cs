using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Model;

public class Tag : IEntity<int>
{
#pragma warning disable CS8618
    protected Tag()
    {
    }
#pragma warning restore CS8618

    public Tag(string name, Category category)
    {
        Name = name;
        Category = category;
        Documents = new List<DocumentTag>();
    }
    [Key] public int Id { get; protected set; }
    [Required] [MaxLength(255)] public string Name { get; protected set; }
    [Required] public Category Category { get; protected set; }
    public List<DocumentTag> Documents { get; set; }
}