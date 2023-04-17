using System;
using System.Text.Json.Serialization;
using DocumentManager.Dto;
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
    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpContextAccessor();

//this fixed the cycles of documentTag (quick fix but if it works, it works)
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// byte[] secret = Convert.FromBase64String(builder.Configuration["Secret"]);
// builder.Services
//     .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(secret),
//             ValidateAudience = false,
//             ValidateIssuer = false
//         };
//     });
// ***************************************** APPLICATION ******************************************
var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    try
    {
        await app.UsePostgresContainer(
            "documentmanager_postgres", "latest",
            app.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException(),
            true);
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
        db.CreateDatabase(app.Environment.IsDevelopment());
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();