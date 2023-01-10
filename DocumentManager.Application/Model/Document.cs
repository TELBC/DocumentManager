using System.ComponentModel.DataAnnotations;
namespace DocumentManager.Model;

public class Document : IEntity<int>
{
    public Document(string title, string content, List<Tag> tags, string type, Folder folder)
    {
        Title = title;
        Content = content;
        Tags = tags;
        Type = type;
        Folder = folder;
    }

    #pragma warning disable CS8618
    protected Document() { }
    #pragma warning restore CS8618

    [Key]
    public int Id { get; private set; }

    private string _title;
    [Required]
    public string Title
    {
        get => _title;
        set { _title = value; _version++; }
    }
    private string _content;
    [ConcurrencyCheck]
    public string Content
    {
        get => _content;
        set { _content = value; _version++; }
    }
    private List<Tag> _tags;
    public List<Tag> Tags
    {
        get => _tags;
        set { _tags = value; _version++; }
    }
    private string _type;
    [Required]
    public string Type
    {
        get => _type;
        set { _type = value; _version++; }
    }
    private int _version;
    public int Version { get => _version; }
    public Folder Folder { get; set; }
}

//updated to what is required in the project