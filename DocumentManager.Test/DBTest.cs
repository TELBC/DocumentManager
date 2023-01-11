using DocumentManager.Infrastructure;
using Xunit;

namespace DocumentManager.Test;

public class DBTest : DocumentManagerDB
{
    [Fact]
    public void CreateDatabaseTest()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
    }
    
}