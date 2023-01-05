using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class Folder
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Document> Documents { get; set; }

    public Folder(string name)
    {
        Name = name;
        Documents = new List<Document>();
    }
    
    protected Folder(){}

    public void AddDoc(Document document)
    {
        Documents.Add(document);
    }

    public void RemoveDoc(Document document)
    {
        Documents.Remove(document);
    }
}