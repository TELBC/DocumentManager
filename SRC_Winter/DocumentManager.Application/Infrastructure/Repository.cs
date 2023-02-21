using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Infrastructure;

public class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> Db;

    public Repository(DbContext dbContext)
    {
        _context = dbContext;
        Db = dbContext.Set<TEntity>();
    }
    
#pragma warning disable CS8618
    protected Repository(){}
#pragma warning restore CS8618

    public IQueryable<TEntity> Queryable => Db.AsQueryable();

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