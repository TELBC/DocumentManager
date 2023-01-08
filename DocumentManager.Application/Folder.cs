using System.ComponentModel.DataAnnotations;

namespace DocumentManager;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Document> Documents { get; set; }

    public Folder() { }

    public Folder(string name)
    {
        Name = name;
    }

    public void AddDoc(Document document)
    {
        Documents.Add(document);
    }

    public void RemoveDoc(Document document)
    {
        Documents.Remove(document);
    }
}