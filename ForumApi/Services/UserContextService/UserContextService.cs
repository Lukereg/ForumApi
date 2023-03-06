using ForumApi.Exceptions;
using System.Security.Claims;

namespace ForumApi.Services.UserContextService
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? getCurrentUserOrDefault()
            => _httpContextAccessor?.HttpContext?.User;

        private Claim? getCurrentUserIdOrDefault()
            => getCurrentUserOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        public ClaimsPrincipal GetUser()
        {
            var user = getCurrentUserOrDefault();

            if (user is null)
                throw new UnauthorizedException("User does not exist");

            return user;
        }

        public int GetUserId()
        {
            var idUser = getCurrentUserIdOrDefault();

            if (idUser == null)
                throw new UnauthorizedException("Id claim does not exist");

            return int.Parse(idUser.Value);
        }
    }
}
