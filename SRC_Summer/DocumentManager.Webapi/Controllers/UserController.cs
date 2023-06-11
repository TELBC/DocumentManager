using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DocumentManager.Webapi.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly DocumentManagerContext _db;
    private readonly IConfiguration _config;
    public record CredentialsDto(string username, string password);

    public UserController(DocumentManagerContext db,IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    //----------------------------------------------------------------------------------------------
    [HttpPost("login")]
    public IActionResult Login([FromBody] CredentialsDto credentials)
    {
        var secret = Convert.FromBase64String(_config["Secret"]!);
        var lifetime = TimeSpan.FromHours(3);
        var user = _db.User.FirstOrDefault(a => a.Name == credentials.username);
        if (user is null) { return Unauthorized(); }
        if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }

        string role = "Admin";
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            }),
            Expires = DateTime.UtcNow + lifetime,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new
        {
            user.Name,
            UserGuid = user.Guid,            
            Role = role,
            Token = tokenHandler.WriteToken(token)
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUserdata()
    {
        var username = HttpContext?.User.Identity?.Name;
        if (username is null) { return Unauthorized(); }
        var user = _db.User.FirstOrDefault(a => a.Name == username);
        if (user is null) { return Unauthorized(); }
        return Ok(new
        {
            user.Name,
            user.Email,
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllUsers()
    {
        var user = _db.User
            .Select(user => new
            {
                user.Name,
                user.Email,
            })
            .ToList();
        if (user is null) { return BadRequest(); }
        return Ok(user);
    }
}