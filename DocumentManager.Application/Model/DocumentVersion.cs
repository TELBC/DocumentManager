namespace DocumentManager.Model;

public class DocumentVersion
{
    public int Version { get; set; }
    public String Description { get; set; }
    public DateTime Date { get; set; }
    public User Author { get; set; }
    public String Content { get; set; }

    public DocumentVersion(int version, string description, DateTime date, User author, string content)
    {
        Version = version;
        Description = description;
        Date = date;
        Author = author;
        Content = content;
    }
}