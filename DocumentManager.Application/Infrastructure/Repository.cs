using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    protected readonly DbSet<TEntity> _db;
    public IQueryable<TEntity> queryable => _db.AsQueryable();
    private DbContext _context;

    public Repository(DbContext documentManagerContext)
    {
        _context = documentManagerContext;
        _db = documentManagerContext.Set<TEntity>();
    }
    
    public virtual void InsertOne(TEntity e)
    {
        _db.Add(e);
        _context.SaveChanges();
    }

    public virtual void DeleteOne(TKey id)
    {
        var e = _db.Find(id);
        if (e is not null) _db.Remove(e);
        _context.SaveChanges();
    }
    
    public virtual void UpdateOne(TEntity element)
    {
        var e = _db.Find(element.Id);
        if (e is not null) _db.Update(element);
        _context.SaveChanges();
    }
}