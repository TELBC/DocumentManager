using System.Linq;
using DocumentManager.Infrastructure;
using DocumentManager.Model;
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
    public IActionResult GetAllUsers()
    {
        var users = _db.UserBase.ToList();
        return Ok(users);
    }
    
    
    // Reacts to /api/users/1
    [HttpGet("{id:int}")]
    public IActionResult GetUserDetail(int id)
    {
        var user = _db.UserBase.Select(x =>new 
        {
            id = x.Id,
            name = x.Name,
            email = x.Email
        }).FirstOrDefault(u => u.id == id);

        if (user is null) return NotFound();
        return Ok(user);
    }
}