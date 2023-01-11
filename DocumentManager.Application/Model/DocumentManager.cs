using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Model;

[Table("DocumentManager")]
public class DocumentManager : IEntity<int>
{
    public DocumentManager(User user)
    {
        User = user;
    }
    #pragma warning disable CS8618 
    protected DocumentManager() { }
    #pragma warning restore CS8618

    [Key]
    public int Id { get; set; }
    
    [Required]
    public User User { get; set; }

    // private List<UserBase> _friends = new();
    // private List<Folder> _folders = new();
    // public virtual IReadOnlyCollection<UserBase> Users => _friends;
    //
    // public virtual IReadOnlyCollection<Folder> Folders => _folders;
    //
    // public void AddFolder(Folder folder)
    // {
    //     _folders.Add(folder);
    // }
    //
    //
    // public void AddUser(UserBase newUser)
    // {
    //     _friends.Add(newUser);
    // }
}