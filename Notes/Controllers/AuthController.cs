using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        List<Claim> claims = new List<Claim>
        {
            new Claim("id",userId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678123456781234567812345678"));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpPost]
    public ActionResult SignIn([FromForm] UserDto payload)
    {
        var user = this._dbContext.Users.Single(u => u.Username == payload.Username);
        
        Console.Write(user);
        if(user is null)  return Unauthorized("Invalid username or password");
        Console.Write(BCrypt.Net.BCrypt.HashPassword(payload.Password, 12));
        bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(payload.Password, user.Password);
        if (!isPasswordMatch) return Unauthorized("Invalid username or password");

        string jwtToken = this.CreateJwt(user.Id);
        
        return Ok(new { jwtToken, username = user.Username });
    }
}