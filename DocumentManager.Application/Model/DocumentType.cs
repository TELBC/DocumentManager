namespace DocumentManager.Model;

public class DocumentType
{
    public String Title { get; set; }
    public String Description { get; set; }

    public DocumentType(string title, string description)
    {
        Title = title;
        Description = description;
    }
}