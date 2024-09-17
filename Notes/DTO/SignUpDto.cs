using System.ComponentModel.DataAnnotations;

namespace Notes.DTO;

public class SignUpDto
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}