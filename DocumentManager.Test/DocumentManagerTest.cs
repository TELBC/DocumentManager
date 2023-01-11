using Bogus;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Xunit;

namespace DocumentManager.Test;

public class DocumentManagerTest : DocumentManagerDB
{
    public DocumentManagerTest()
    {
        _db.Database.EnsureCreated();
        var document = new Faker<Document>().CustomInstantiator(fa =>
                new Document(title:fa.Commerce.ProductName(),content:fa.Lorem.Sentence(),tags:new List<Tag>(),type:"."+fa.System.FileType()))
            .RuleFor(d => d.Tags, 
                f => f.Make(f.Random.Int(1, 4), () => new Tag(f.Commerce.ProductName(),f.Random.Enum<Category>())))
            .Generate();

        _db.Add(document);
        
        _db.SaveChanges();

    }
    // [Fact]
    // public void AddDocumentTest()
    // {
    //     var document = _db.Documents.First();
    //     var docMan = new Model.DocumentManager(new User("test","test","test"));
    //     docMan.AddDocument(document);
    //     _db.SaveChanges();
    //     _db.ChangeTracker.Clear();
    //     
    //     var doc = _db.Documents.First();
    //     
    //
    // }
}