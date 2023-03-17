using System.Linq;
using DocumentManager.Infrastructure;
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
        var users = _db.User.ToList();
        return Ok(users); // need to change Userbase to User relationship to make it get a key that maps to the password instead of plaintext
    }
    
    // Reacts to /api/users/1
    [HttpGet("{id:int}")]
    public IActionResult GetUserDetail(int id)
    {
        var user = _db.User.FirstOrDefault(u => u.Id == id);

        if (user is null) return NotFound();
        return Ok(user);
    }
}