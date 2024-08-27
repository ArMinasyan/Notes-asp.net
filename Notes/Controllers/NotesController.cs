using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Route("/notes")]
[Authorize]
public class NotesController : Controller
{
    private readonly DatabaseContext _dbContext;
    private readonly AuthUser _user = new AuthUser();

    public NotesController(DatabaseContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public ActionResult GetAllNotes()
    {
        var user = this._user.GetUser();
        var notes = this._dbContext.Notes
            .Where(note => note.UserId == user.Id)
            .Select(note => new { id = note.Id, title = note.Title, description = note.Description })
            .ToList();
        return Ok(notes);
    }

    [HttpPost]
    public IActionResult CreateNote([FromBody] NoteDto payload)
    {
        Paylod user = this._user.GetUser();
        var note = new NoteModel { Title = payload.Title, Description = payload.Description, UserId = user.Id };
        this._dbContext.Notes.Add(note);
        this._dbContext.SaveChanges();
        return StatusCode((int)HttpStatusCode.Created, note);
    }

    [HttpPut(":id")]
    public IActionResult UpdateNote(int id, [FromBody] NoteDto payload)
    {
        Paylod user = this._user.GetUser();
        var note = this._dbContext.Notes.FirstOrDefault(note => note.Id == id && note.UserId == user.Id);

        if (note is null)
        {
            return StatusCode((int)HttpStatusCode.NotFound, new { message = "Note not found." });
        }

        note.Title = payload.Title ?? note.Title;
        note.Description = payload.Description ?? note.Description;

        this._dbContext.SaveChanges();
        return StatusCode((int)HttpStatusCode.OK, new { id });
    }

    [HttpDelete(":id")]
    public IActionResult DeleteNote(int id)
    {
        Paylod user = this._user.GetUser();
        var deletedNote = this._dbContext.Notes
            .Where(note => note.Id == id)
            .Where(note => note.UserId == user.Id)
            .ExecuteDelete();

        if (deletedNote == 0)
        {
            return StatusCode((int)HttpStatusCode.NotFound, new { message = "Note not found." });
        }

        return StatusCode((int)HttpStatusCode.OK, new { id });
    }
}