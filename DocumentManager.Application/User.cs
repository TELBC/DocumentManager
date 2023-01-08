using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class User : UserBase
{
    [Required]
    public string Password { get; set; }

    public int FolderId { get; set; }

    [ForeignKey("FolderId")]
    public Folder Folder { get; set; }

    protected User() { }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public void ShareDoc(Document document, List<User> users)
    {
        document.Folder.AddDoc(document);
        users.ForEach(u => u.Folder.AddDoc(document));
    }

    public void RevokeAcc(Document document, List<User> users)
    {
        document.Folder.RemoveDoc(document);
        users.ForEach(u => u.Folder.RemoveDoc(document));
    }
}