using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Notes.DTO;
using Notes.Models;

namespace Notes.Controllers;

public class AuthController(DatabaseContext dbContext, IConfiguration configuration) : BaseApiController
{
    private string CreateJwt(int userId)
    {
        var JwtSettings = configuration.GetSection("JwtSettings");

        List<Claim> claims = new List<Claim>
        {
            new Claim("id", userId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Secret"]!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims ,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: signingCredentials,
            issuer: JwtSettings["ValidIssuer"]
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpPost]
    public ActionResult SignIn([FromBody] UserDto payload)
    {
        var user = dbContext.Users.SingleOrDefault(u => u.Username == payload.Username);
        if(user is null)  return Unauthorized("Invalid username and/or password");
        bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(payload.Password, user.Password);
        if (!isPasswordMatch) return Unauthorized("Invalid username and/or password");

        string jwtToken = this.CreateJwt(user.Id);
        
        return Ok(new { jwtToken, username = user.Username, id = user.Id });
    }
}