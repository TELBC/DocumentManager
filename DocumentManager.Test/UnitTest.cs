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

            // folder creation
            var folder = new Folder("New Folder");
            context.Folders.Add(folder);

            // document creation
            var doc = new Document("Document 1", "This is a test",
                new List<Tag> { new Tag("test", Category.Miscellaneous) }, "txt", folder);
            context.Documents.Add(doc);
            
            // user creation
            var user = new User("John Doe", "johndoe@example.com", "qwerty123");
            context.Users.Add(user);
            
            folder.Documents.Add(doc);
            var docManager = new DocumentManager(new List<User> { user }, new List<GuestUser>(), new List<Folder> { folder });
            context.DocumentManagers.Add(docManager);
            
            // saves everything
            context.SaveChanges();

            // update doc
            doc.UpdateTitle("Updated Document 1");
            Assert.Equal("Updated Document 1", doc.Title);
            Assert.Equal(2, doc.Version);
            doc.UpdateContent("This is an updated test");
            Assert.Equal("This is an updated test", doc.Content);
            Assert.Equal(3, doc.Version);
            // add doc tags
            doc.AddTags(new List<Tag> { new Tag("update", Category.Personal) });
            Assert.Contains(doc.Tags, t => t.Name == "update");
            Assert.Equal(4, doc.Version);
            // remove doc tags
            doc.RemoveTags(new List<Tag> { doc.Tags.First(t => t.Name == "test") });
            Assert.DoesNotContain(doc.Tags, t => t.Name == "test");
            Assert.Equal(5, doc.Version);
            // update doc type
            doc.UpdateType("md");
            Assert.Equal("md", doc.Type);
            Assert.Equal(6, doc.Version);
            // move to new folder(1)
            doc.MvFolder(new Folder("New Folder(1)"));
            Assert.Equal("New Folder(1)", doc.Folder.Name);
            Assert.Equal(7, doc.Version);
            
            // test user actions
            user.ShareDoc(doc, new List<User> { new User("Jane Doe", "janedoe@example.com", "qwerty123" ) });
            Assert.Contains(doc.Users, u => u.Name == "Jane Doe");

            user.RevokeAcc(doc, new List<User> { doc.Users.First(u => u.Name == "Jane Doe") });
            Assert.DoesNotContain(doc.Users, u => u.Name == "Jane Doe");

            // test document manager actions
            docManager.CreateDocument("Document 2", "This is a new test 2", 
                new List<Tag> { new Tag( "new", Category.Work ) }, "txt", folder);
            Assert.Equal(1, context.Documents.Count());

            docManager.DeleteDoc(doc);
            Assert.Equal(1, context.Documents.Count());

            //Assert.Equal(1, docManager.SearchDocs("Document 2").Count());
            Assert.Single(docManager.SearchDocs("Document 2"));
            
            docManager.GrantAcc(docManager.SearchDocs("Document 2").First(), 
                new List<User> { new User("Bob Doe","bobdoe@example.com","qwerty123")});
            Assert.Contains(docManager.SearchDocs("Document 2").First().Users, u => u.Name == "Bob Doe");

            docManager.RevokeAcc(docManager.SearchDocs("Document 2").First(), 
                new List<User> { docManager.SearchDocs("Document 2").First().Users.First(u => u.Name == "Bob Doe") });
            Assert.DoesNotContain(docManager.SearchDocs("Document 2").First().Users, u => u.Name == "Bob Doe");
        }
    }
}