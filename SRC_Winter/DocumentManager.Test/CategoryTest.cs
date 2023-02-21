using Bogus;
using DocumentManager.Model;
using Xunit;

namespace DocumentManager.Test;
[Collection("Sequential")]
public class CategoryTest : DocumentManagerDb
{
    [Fact]
    public void ValueConverterTest()
    {
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        var tagPersonal = new Faker<Tag>().CustomInstantiator(faker =>
            new Tag(faker.Lorem.Word(), Category.Personal)).Generate();
        Db.Add(tagPersonal);
        Db.SaveChanges();

        Assert.True(Db.Tag.First().Category == Category.Personal);
    }
}