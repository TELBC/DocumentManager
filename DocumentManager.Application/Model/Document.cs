namespace DocumentManager.Model;

public class Document
{
    public int DocID { get; private set; }
    public String Title { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public List<DocumentTag> Tag { get; set; }
    public int Size { get; set; }
    public User Author { get; set; }
    public List<DocumentVersion> Version { get; set; }
    public Category Category { get; set; }

    public Document(int docId, string title, List<DocumentTag> tag, User author, List<DocumentVersion> version, Category category)
    {
        DocID = docId;
        Title = title;
        Tag = tag;
        Author = author;
        Version = version;
        Category = category;
    }
}