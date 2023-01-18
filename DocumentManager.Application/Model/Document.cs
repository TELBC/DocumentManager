using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Document : IEntity<int>
{
    private string _content;
    private List<Tag> _tags;

    private string _title;
    private string _type;

    public Document(string title, string content, List<Tag> tags, string type)
    {
        Title = title;
        Content = content;
        Tags = tags;
        Type = type;
    }

#pragma warning disable CS8618
    protected Document()
    {
    }
#pragma warning restore CS8618
    [Required]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            Version++;
        }
    }

    [ConcurrencyCheck]
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            Version++;
        }
    }

    public List<Tag> Tags
    {
        get => _tags;
        set
        {
            _tags = value;
            Version++;
        }
    }

    [Required]
    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            Version++;
        }
    }

    public int Version { get; private set; }

    [Key] public int Id { get; private set; }
}