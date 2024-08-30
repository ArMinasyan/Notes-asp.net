using Notes.DTO;

namespace Notes;

public class AuthUser
{
    private readonly IHttpContextAccessor? _httpContextAccessor = new HttpContextAccessor();

    public AuthUser()
    {
    }

    public Paylod GetUser()
    {
        return new Paylod
        {
            Id = Convert.ToInt16(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault()?.Value)
        };
    }
}