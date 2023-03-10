using System;
using DocumentManager.Infrastructure;
using DocumentManager.Webapi;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DocumentManagerContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
}



// ***************************************** APPLICATION ******************************************
var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
     try
     {
         await app.UsePostgresContainer(
             containerName: "documentmanager_postgres", version: "latest",
             connectionString: app.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException(),
             deleteAfterShutdown: true);
     }
     catch (Exception e)
     {
         app.Logger.LogError(e.Message);
         return;
     }
     app.UseCors();
}

using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<DocumentManagerContext>())
    {
        db.CreateDatabase(isDevelopment: app.Environment.IsDevelopment());
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
