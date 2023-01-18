using DocumentManager.Model;

namespace DocumentManager.Infrastructure;

public class DocumentManagerRepository : Repository<Model.DocumentManager, int>
{
    public DocumentManagerRepository(DocumentManagerContext context) : base(context)
    {
    }

    public void AddFolder(int id, Folder folder)
    {
        var entity = _db.Find(id);
        if (entity is not null)
        {
            entity.AddFolder(folder);
            UpdateOne(entity);
        }
    }

    public void AddFriend(int id, UserBase friend)
    {
        var entity = _db.Find(id);
        if (entity is not null)
        {
            entity.AddFriend(friend);
            UpdateOne(entity);
        }
    }
}