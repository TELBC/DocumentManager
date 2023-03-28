using System;
using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Webapi.Controllers;

using System.Linq;
using DocumentManager.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[Route("/api/tags")]
[ApiController]
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
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                category = x.Category
            })
            .ToList();
        return Ok(tags);
    }
    // Reacts to /api/tags/1
    [HttpGet("{id:int}")]
    public IActionResult GetTagDetail(int id) 
    {
        var tag = _db.Tag
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                category = x.Category
            })
            .OrderBy(x => x.id)
            .FirstOrDefault(x => x.id==id);
        if (tag is null) return NotFound();
        return Ok(tag);
    }
}