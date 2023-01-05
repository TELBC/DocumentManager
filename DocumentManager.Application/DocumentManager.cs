using System.ComponentModel.DataAnnotations;

namespace DocumentManager;

public class DocumentManager
{
    [Key]
    public int Id { get; set; }
    public List<User> Users { get; set; }
    public List<GuestUser> GuestUsers { get; set; }
    public List<Folder> Folders { get; set; }

    public DocumentManager(List<User> users = default, List<GuestUser> guestUsers = default, List<Folder> folders = default)
    {
        Users = users ?? new List<User>();
        GuestUsers = guestUsers ?? new List<GuestUser>();
        Folders = folders ?? new List<Folder>();
    }

    public Document CreateDocument(string title, string content, List<Tag> tags, string type, Folder folder)
    {
        var doc = new Document(title, content, tags, type, folder);
        folder.Documents.Add(doc);
        return doc;
    }

    public void DeleteDoc(Document document)
    {
        document.Folder.Documents.Remove(document);
    }

    public List<Document> SearchDocs(string docName)
    {
        return Folders.SelectMany(f => f.Documents).Where(d => d.Title == docName).ToList();
    }

    public void GrantAcc(Document document, List<User> users)
    {
        document.Users.AddRange(users);
        foreach (var u in users)
        {
            u.Documents.Add(document);
        }
    }

    public void RevokeAcc(Document document, List<User> users)
    {
        document.Users.RemoveAll(users.Contains);
        foreach (var u in users)
        {
            u.Documents.Remove(document);
        }
    }
}