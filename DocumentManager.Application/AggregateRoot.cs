using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager;

public class AggregateRoot
{
    public int Id { get; set; }
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
