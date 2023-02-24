using DocumentManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DocumentManagerContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Port=5432;Database=DocumentManager;Username=postgres;Password=pwd;IncludeErrorDetail=true;")));

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
    app.UseCors();
}

using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<DocumentManagerContext>())
    {
        db.CreateDatabase(isDevelopment: app.Environment.IsDevelopment());
        // db.Database.EnsureCreated();
    }
}

app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
