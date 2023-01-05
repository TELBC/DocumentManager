using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;
public class Document
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Tag> Tags { get; set; }
    public string Type { get; set; }
    public int Version { get; set; }
    public Folder Folder { get; set; }
    public List<User> Users { get; set; }

    public Document(string title, string content, List<Tag> tags, string type, Folder folder)
    {
        Title = title;
        Content = content;
        Tags = tags;
        Type = type;
        Version = 1;
        Folder = folder;
        Users = new List<User>();
    }
    
    protected Document(){}

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
        Tags.AddRange(newTags);
        Version++;
    }

    public void RemoveTags(List<Tag> tagsToRemove)
    {
        Tags.RemoveAll(tagsToRemove.Contains);
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