using DocumentManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Test;

public class DBTest
{
    protected readonly DocumentManagerContext _db;

    public DBTest()
    {
        var opt = new DbContextOptionsBuilder<DocumentManagerContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=pwd;IncludeErrorDetail=true;")
            .Options;

        _db = new DocumentManagerContext(opt);
    }
}