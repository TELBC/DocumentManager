namespace DocumentManager.Model;

public class DocumentManager : AggregateRoot
{
    private readonly ICollection<User> _users;
    private readonly ICollection<GuestUser> _guestUsers;
    private readonly ICollection<Folder> _folders;
    public ICollection<object> ManagedItems { get; set; }

    public DocumentManager()
    {
        _users = new List<User>();
        _guestUsers = new List<GuestUser>();
        _folders = new List<Folder>();
        ManagedItems = new List<object>();
    }

    public IReadOnlyList<User> Users => _users.ToList();
    public IReadOnlyList<GuestUser> GuestUsers => _guestUsers.ToList();
    public IReadOnlyList<Folder> Folders => _folders.ToList();

    // public Document CreateDocument(
    //     string title,
    //     string content,
    //     List<Tag> tags,
    //     string type,
    //     Folder folder)
    // {
    //     var document = new Document(title, content, tags, type, folder);
    //     _users.First(u => u.Id == folder.Id).Folder.AddDoc(document);
    //     return document;
    // }
    //
    // public void DeleteDoc(Document document)
    // {
    //     var folder = _folders.First(f => f.Id == document.FolderId);
    //     folder.RemoveDoc(document);
    // }
    //
    // public List<Document> SearchDocs(string docName)
    // {
    //     return _folders
    //         .SelectMany(f => f.Documents)
    //         .Where(d => d.Title.Contains(docName))
    //         .ToList();
    // }
    //
    // public void GrantAcc(Document document, List<User> users)
    // {
    //     users.ForEach(u => u.Folder.AddDoc(document));
    // }
    //
    // public void RevokeAcc(Document document, List<User> users)
    // {
    //     users.ForEach(u => u.Folder.RemoveDoc(document));
    // }
    //
    // public void Add(User user)
    // {
    //     _users.Add(user);
    // }
    //
    // public void Add(GuestUser guestUser)
    // {
    //     _guestUsers.Add(guestUser);
    // }
    //
    // public void Add(Folder folder)
    // {
    //     _folders.Add(folder);
    // }
}