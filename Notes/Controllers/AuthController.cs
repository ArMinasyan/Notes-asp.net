using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

[Route("/auth")]
public class AuthController : Controller
{
    private readonly DatabaseContext _dbContext;

    public AuthController(DatabaseContext context)
    {
        _dbContext = context;
    }

    private string CreateJwt(int userId)
    {
        List<Claim> claims = new List<Claim> { };

        claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678123456781234567812345678"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    public ActionResult SignIn([FromForm] UserDto payload)
    {
        var user = this._dbContext.Users.SingleOrDefault(u => u.Username == payload.Username);
        if (user is null ||
            user.Password != payload.Password) return Unauthorized("Invalid usernam or password");

        return Ok(new { jwt = this.CreateJwt(user.Id) });
    }
}