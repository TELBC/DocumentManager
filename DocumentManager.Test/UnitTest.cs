using System.Linq;
using Xunit;
using DocumentManager.Context;
using Microsoft.EntityFrameworkCore;
using Assert = Xunit.Assert;

namespace DocumentManager.Test;

public class UnitTest
{
    [Fact]
    public void TestModelFunctionality()
    {
        var options = new DbContextOptionsBuilder<DocumentManagerContext>()
            .UseInMemoryDatabase(databaseName: "DocumentManagementDatabase")
            .Options;
        using (var context = new DocumentManagerContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // create a folder
            var folder = new Folder("Test Folder");
            context.Folders.Add(folder);
            context.SaveChanges();

            // create a document
            var doc = new Document("Test Document", "This is a test document",
                new List<Tag> { new Tag("test", Category.Miscellaneous) }, "text", folder);
            context.Documents.Add(doc);
            context.SaveChanges();

            // create a user
            var user = new User("John Smith", "john@example.com", "password");
            context.Users.Add(user);
            context.SaveChanges();

            // add the document to the folder
            folder.Documents.Add(doc);
            context.SaveChanges();

            // create a document manager
            var docManager = new DocumentManager(new List<User> { user }, new List<GuestUser>(), new List<Folder> { folder });
            context.DocumentManagers.Add(docManager);
            context.SaveChanges();

            // test document updates
            doc.UpdateTitle("Updated Test Document");
            Assert.Equal("Updated Test Document", doc.Title);
            Assert.Equal(2, doc.Version);

            doc.UpdateContent("This is an updated test document");
            Assert.Equal("This is an updated test document", doc.Content);
            Assert.Equal(3, doc.Version);

            doc.AddTags(new List<Tag> { new Tag("update", Category.Personal) });
            Assert.Contains(doc.Tags, t => t.Name == "update");
            Assert.Equal(4, doc.Version);

            doc.RemoveTags(new List<Tag> { doc.Tags.First(t => t.Name == "test") });
            Assert.DoesNotContain(doc.Tags, t => t.Name == "test");
            Assert.Equal(5, doc.Version);

            doc.UpdateType("rich text");
            Assert.Equal("rich text", doc.Type);
            Assert.Equal(6, doc.Version);

            doc.MvFolder(new Folder("Updated Test Folder"));
            Assert.Equal("Updated Test Folder", doc.Folder.Name);
            Assert.Equal(7, doc.Version);
            // test user actions
            user.ShareDoc(doc, new List<User> { new User("Jane Smith", "jane@example.com", "password" ) });
            Assert.Contains(doc.Users, u => u.Name == "Jane Smith");

            user.RevokeAcc(doc, new List<User> { doc.Users.First(u => u.Name == "Jane Smith") });
            Assert.DoesNotContain(doc.Users, u => u.Name == "Jane Smith");

            // test document manager actions
            docManager.CreateDocument("New Test Document", "This is a new test document", 
                new List<Tag> { new Tag( "new", Category.Work ) }, "text", folder);
            Assert.Equal(2, context.Documents.Count());

            docManager.DeleteDoc(doc);
            Assert.Equal(1, context.Documents.Count());

            Assert.Equal(1, docManager.SearchDocs("New Test Document").Count());

            docManager.GrantAcc(docManager.SearchDocs("New Test Document").First(), 
                new List<User> { new User("Bob Smith","bob@example.com","password")});
            Assert.Contains(docManager.SearchDocs("New Test Document").First().Users, u => u.Name == "Bob Smith");

            docManager.RevokeAcc(docManager.SearchDocs("New Test Document").First(), 
                new List<User> { docManager.SearchDocs("New Test Document").First().Users.First(u => u.Name == "Bob Smith") });
            Assert.DoesNotContain(docManager.SearchDocs("New Test Document").First().Users, u => u.Name == "Bob Smith");
        }
    }
}