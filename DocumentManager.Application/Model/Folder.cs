﻿using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Folder : IEntity<int>
{
    public Folder(string name, List<Document>? documents)
    {
        Name = name;
        Documents = documents;
    }
#pragma warning disable CS8618
    public Folder()
    {
    }
#pragma warning restore CS8618
    [Required] [MaxLength(255)] public string Name { get; set; }

    [Required] public List<Document> Documents { get; set; }

    [Key] public int Id { get; set; }
}