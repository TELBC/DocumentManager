using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class Document
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Content { get; set; }

    public List<DocumentTag> DocumentTags { get; set; }

    public string Type { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Version { get; set; }

    public int FolderId { get; set; }

    [ForeignKey("FolderId")]
    public Folder Folder { get; set; }

    protected Document() { }

    public Document(string title, string content, List<Tag> tags, string type, Folder folder)
    {
        Title = title;
        Content = content;
        DocumentTags = tags.Select(t => new DocumentTag { Tag = t }).ToList();
        Type = type;
        Folder = folder;
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
        Version++;
    }

    public void UpdateContent(string newContent)
    {
        Content = newContent;
        Version++;
    }

    public void AddTags(List<Tag> newTags)
    {
        DocumentTags.AddRange(newTags.Select(t => new DocumentTag { Tag = t }));
        Version++;
    }

    public void RemoveTags(List<Tag> tagsToRemove)
    {
        DocumentTags = DocumentTags
            .Where(dt => !tagsToRemove.Contains(dt.Tag))
            .ToList();
        Version++;
    }

    public void UpdateType(string newType)
    {
        Type = newType;
        Version++;
    }

    public void MvFolder(Folder newFolder)
    {
        Folder = newFolder;
        Version++;
    }
}