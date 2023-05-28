using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Model;

public class Folder : IEntity<int>
{
    public Folder(string name)
    {
        Name = name;
        Documents = new List<Document>();
    }
    #pragma warning disable CS8618
    public Folder() { }
    #pragma warning restore CS8618
    
    [Key] public int Id { get; set; }
    
    public Guid Guid { get; set; }
    
    [Required] [MaxLength(255)] public string Name { get; set; }

    public List<Document>? Documents { get; set; }
    [ForeignKey("Id")]//have to declare the foreign key since i need it for the controllers
    public int? DocumentManagerId { get; set; }
}