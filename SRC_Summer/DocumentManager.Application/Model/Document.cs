using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Document : IEntity<int>
{
    public Document(string title, string content, string type)
    {
        Title = title;
        Content = content;
        Tags = new List<DocumentTag>();
        Type = type;
    }

    #pragma warning disable CS8618
    protected Document() { }
    #pragma warning restore CS8618
    
    public string Title { get; set; }

    [ConcurrencyCheck]
    public string Content { get; set; }
    public List<DocumentTag> Tags { get; set; }
    public string Type{ get; set; }

    public int Version { get; private set; }

    [Key] public int Id { get; private set; }
}