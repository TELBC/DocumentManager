using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Model;

public class AggregateRoot : IEntity<int>
{
    public int Id { get; private set; }
    public ICollection<object> ManagedItems { get; set; }

    public AggregateRoot()
    {
        ManagedItems = new List<object>();
    }

    public void Add(object item, DbContext context)
    {
        ManagedItems.Add(item);
        context.Entry(this).Property("ManagedItems").CurrentValue = ManagedItems;
    }
}
