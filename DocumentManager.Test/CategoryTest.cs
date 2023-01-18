using Bogus;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace DocumentManager.Test;

public class CategoryTest : DocumentManagerDB
{
    [Fact]
    public void ValueConverterTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        var tagPersonal = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(faker.Lorem.Word(), Category.Personal)).Generate();
        _db.Add(tagPersonal);
        _db.SaveChanges();

        Assert.True(_db.Tag.First().Category == Category.Personal);
    }
}