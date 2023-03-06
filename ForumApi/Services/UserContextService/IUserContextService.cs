using System.Security.Claims;

namespace ForumApi.Services.UserContextService
{
    public interface IUserContextService
    {
        public int GetUserId();
        public ClaimsPrincipal GetUser();
    }
}