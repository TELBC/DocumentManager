using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    protected readonly DbSet<TEntity> Db;
    public IQueryable<TEntity> Queryable => Db.AsQueryable();
    private readonly DbContext _context;

    public Repository(DbContext dbContext)
    {
        _context = dbContext;
        Db = dbContext.Set<TEntity>();
    }
    
    public virtual void InsertOne(TEntity e)
    {
        Db.Add(e);
        _context.SaveChanges();
    }

    public virtual void DeleteOne(TKey id)
    {
        var e = Db.Find(id);
        if (e is not null) Db.Remove(e);
        _context.SaveChanges();
    }
    
    public virtual void UpdateOne(TEntity element)
    {
        var e = Db.Find(element.Id);
        if (e is not null) Db.Update(element);
        _context.SaveChanges();
    }
}