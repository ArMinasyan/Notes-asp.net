using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Route("/notes")]
[Authorize]
public class NotesController: Controller
{
    private readonly DatabaseContext _dbContext;
    private readonly GetUser user = new GetUser();
    
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
        Paylod user = this.user.Get();
        var note = new NoteModel { Title = payload.Title, Description = payload.Description, UserId = user.id };
        this._dbContext.Notes.Add(note);
        this._dbContext.SaveChanges();
        return StatusCode((int) HttpStatusCode.Created, note);
    }

}