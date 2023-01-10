using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentManager.Model;

public class AggregateRootConfiguration : IEntityTypeConfiguration<AggregateRoot>
{
    public void Configure(EntityTypeBuilder<AggregateRoot> builder)
    {
        builder.HasKey(ar => ar.Id);

        builder.Property(p => p.ManagedItems)
            .HasConversion(new ValueConverter<ICollection<object>, string>(
                v => string.Join(',', v.Select(x => x.ToString())),
                v => v.Split(',', StringSplitOptions.None)
                    .Select(x => (object)Convert.ChangeType(x, typeof(object))).ToList()));
    }
}