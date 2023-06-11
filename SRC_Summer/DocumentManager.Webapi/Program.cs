using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DocumentManager.Dto;
using DocumentManager.Infrastructure;
using DocumentManager.Webapi;
using DocumentManager.Webapi.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<DocumentController>();
builder.Services.AddTransient<TagController>();
builder.Services.AddTransient<FolderController>();
builder.Services.AddTransient<UserController>();

builder.Services.AddHttpContextAccessor();

//this fixed the cycles of documentTag (quick fix but if it works, it works)
//fixes m:n cycles
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "DocumentManager WebApp", Version = "v1"});
    c.SwaggerGeneratorOptions.Servers = new List<OpenApiServer> { new() {Url = "https://localhost:5001" } };
});

LogLevel documentManagerLevel = LogLevel.Critical;
builder.Logging.AddFilter("DocumentManager", documentManagerLevel);
builder.Logging.AddDebug();

byte[] secret = Convert.FromBase64String(builder.Configuration["Secret"]);
builder.Services
    .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });
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
    
    // //Swaggermiddleware
    app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    app.UseCors();
}

using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<DocumentManagerContext>())
    {
        db.CreateDatabase(app.Environment.IsDevelopment());
    }
}

app.UseRouting();

app.UseStaticFiles();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentManager WebApp v1"));

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
