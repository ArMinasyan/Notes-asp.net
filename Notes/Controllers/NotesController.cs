using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Authorize]
public class NotesController(DatabaseContext dbContext) : BaseApiController
{
    private readonly AuthUser _authUser = new AuthUser();

    [HttpGet]
    public ActionResult GetAllNotes()
    {
        var user = _authUser.GetUser();
        var notes = dbContext.Notes
            .Where(note => note.UserId == user.Id)
            .Select(note => new { id = note.Id, title = note.Title, description = note.Description })
            .ToList();
        return Ok(notes);
    }

    [HttpPost]
    public IActionResult CreateNote([FromBody] NoteDto payload)
    {
        Paylod user = _authUser.GetUser();
        var note = new NoteModel { Title = payload.Title, Description = payload.Description, UserId = user.Id };
        dbContext.Notes.Add(note);
        dbContext.SaveChanges();
        return StatusCode((int)HttpStatusCode.Created, note);
    }

    [HttpPut(":id")]
    public IActionResult UpdateNote(int id, [FromBody] NoteDto payload)
    {
        Paylod user = _authUser.GetUser();
        var note = dbContext.Notes.FirstOrDefault(note => note.Id == id && note.UserId == user.Id);

        if (note is null)
        {
            return StatusCode((int)HttpStatusCode.NotFound, new { message = "Note not found." });
        }

        note.Title = payload.Title ?? note.Title;
        note.Description = payload.Description ?? note.Description;

        dbContext.SaveChanges();
        return StatusCode((int)HttpStatusCode.OK, new { id });
    }

    [HttpDelete(":id")]
    public IActionResult DeleteNote(int id)
    {
        Paylod user = _authUser.GetUser();
        var deletedNote = dbContext.Notes
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