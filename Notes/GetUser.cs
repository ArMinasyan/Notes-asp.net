using Notes.DTO;

namespace Notes;

public class GetUser
{
    private readonly IHttpContextAccessor? _httpContextAccessor = new HttpContextAccessor();

    public GetUser()
    {
    }


    public Paylod Get()
    {
        return new Paylod
        {
            id = Convert.ToInt16(this._httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value)
        };
    }
}