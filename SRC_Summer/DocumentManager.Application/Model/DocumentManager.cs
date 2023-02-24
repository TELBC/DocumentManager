using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Model;

[Table("DocumentManager")]
public sealed class DocumentManager : IEntity<int>
{
    private readonly List<Folder> _folders = new();

    private readonly List<UserBase> _friends = new();
#pragma warning disable CS8618
    private DocumentManager() { }
#pragma warning restore CS8618
    public DocumentManager(User user)
    {
        User = user;
    }

    [Required] public User User { get; set; }

    public IReadOnlyCollection<UserBase> Users => _friends;

    public IReadOnlyCollection<Folder> Folders => _folders;

    [Key] public int Id { get; set; }

    public void AddFolder(Folder folder)
    {
        _folders.Add(folder);
    }

    public void AddFriend(UserBase newFriend)
    {
        _friends.Add(newFriend);
    }
}