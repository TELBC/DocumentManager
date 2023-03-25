using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Webapi.Controllers;

using System.Linq;
using DocumentManager.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/tags")]
public class TagController : ControllerBase
{
    private readonly DocumentManagerContext _db;

    public TagController(DocumentManagerContext db)
    {
        _db = db;
    }
    // Reacts to GET /api/tags
    [HttpGet]
    public IActionResult GetAllTags()
    {
        var tags = _db.Tag
            .ToList();//fix 
        return Ok(tags);
    }
    // Reacts to /api/tags/1
    [HttpGet("{id:int}")]
    public IActionResult GetTagDetail(int id)//fix 
    {
        var tag = _db.Tag.Find(id);

        if (tag is null) return NotFound();
        return Ok(tag);
    }
}