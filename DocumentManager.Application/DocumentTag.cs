namespace DocumentManager;

public class DocumentTag
{
    public int DocumentId { get; set; }
    public Document Document { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }

    public DocumentTag() { }

    public DocumentTag(Document document, Tag tag)
    {
        Document = document;
        Tag = tag;
    }
}