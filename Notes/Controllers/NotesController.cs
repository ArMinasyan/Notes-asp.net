using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Authorize]
[Route("/notes")]
public class NotesController: Controller
{
    private readonly DatabaseContext _dbContext;

    public NotesController(DatabaseContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public ActionResult GetAllNotes()
    {
        var notes = this._dbContext.Notes.Include(n => n.User).ToList();
        return Ok(notes);
    }

    [HttpGet("get-notes-by-user")]
    public ActionResult GetAllNotesByUser()
    {
        var notes = this._dbContext.Users
            .Where(user => user.Id == 1)
            .Include(user => user.Notes)
            .FirstOrDefault();
        return Ok(notes);
    }
    
    [HttpPost]
    public IActionResult CreateNote([FromBody] NoteDto payload)
    {
        var note = new NoteModel { Title = payload.Title, Description = payload.Description, UserId = 1 };
        this._dbContext.Notes.Add(note);
        this._dbContext.SaveChanges();
        return StatusCode((int) HttpStatusCode.Created, note);
    }

}