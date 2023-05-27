using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Webapi.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly DocumentManagerContext _db;

    public UserController(DocumentManagerContext db)
    {
        _db = db;
    }

    // Reacts to GET /api/users
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _db.UserBase.Select(x => new
        {
            guid = x.Guid,
            name = x.Name,
            email = x.Email
        }).ToListAsync();
        return Ok(users);
    }


    // Reacts to /api/users/1
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserDetail(Guid guid)
    {
        var user = await _db.UserBase.Select(x => new
        {
            guid = x.Guid,
            name = x.Name,
            email = x.Email
        }).FirstOrDefaultAsync(u => u.guid == guid);

        if (user is null) return NotFound();
        return Ok(user);
    }
}