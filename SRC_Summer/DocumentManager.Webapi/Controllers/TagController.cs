using System;
using AutoMapper;
using DocumentManager.Dto;
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
    private readonly IMapper _mapper;

    public TagController(DocumentManagerContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    // -------------------------------------------------------
    // HTTP GET
    // -------------------------------------------------------
    
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
    
    // -------------------------------------------------------
    // HTTP POST
    // -------------------------------------------------------
    
    // [Authorize]
    [HttpPost]
    public IActionResult AddTag(TagDto tagDto)
    {
        var tag = _mapper.Map<Tag>(tagDto);
        if (tag is null) { return NotFound(); }
        _db.Tag.Add(tag);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return Ok(_mapper.Map<Tag, TagDto>(tag));
    }
    
    // -------------------------------------------------------
    // HTTP PUT
    // -------------------------------------------------------
    
    // [Authorize]
    [HttpPut("{id:int}")]
    public IActionResult EditDocument(int id, TagDto tagDto)
    {
        var tag = _db.Tag.FirstOrDefault(a => a.Id == id);
        if (tag is null) { return NotFound(); }
        _mapper.Map(tagDto, tag);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
    
    
    // -------------------------------------------------------
    // HTTP DELETE
    // -------------------------------------------------------
    
    // [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteDocument(int id)
    {
        var tag = _db.Tag.FirstOrDefault(a => a.Id == id);
        if (tag is null) { return NotFound(); }
        _db.Tag.Remove(tag);
        try { _db.SaveChanges(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
}