using System;
using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAllUsers()
    {
        var users = _db.UserBase.Select(x => new
        {
            guid = x.Guid,
            name = x.Name,
            email = x.Email
        }).ToList();
        return Ok(users);
    }


    // Reacts to /api/users/1
    [HttpGet("{guid:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUserDetail(Guid guid)
    {
        var user = _db.UserBase.Select(x => new
        {
            guid = x.Guid,
            name = x.Name,
            email = x.Email
        }).FirstOrDefault(u => u.guid == guid);

        if (user is null) return NotFound();
        return Ok(user);
    }
}