using System.ComponentModel.DataAnnotations;

namespace DocumentManager;

public class UserBase
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Document> Documents { get; set; }

    public UserBase()
    {
        Documents = new List<Document>();
    }

    public void ShareDoc(Document document, List<User> users)
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