using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Route("/auth")]
public class AuthController : Controller
{

    [HttpPost]
    public JsonResult SignIn([FromForm] UserDto payload)
    {
        return new JsonResult(Ok());
    }
}