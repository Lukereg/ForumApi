using ForumApi.Models.Tags;
using ForumApi.Services.UserContextService;
using ForumApi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("v1/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userService;

        public UserRolesController(IUserRolesService userService)
        {
            _userService = userService;
        }

        [HttpPost("{idUser}/roles")]
        public async Task<ActionResult> AddRoleToUser([FromRoute] int idUser, [FromQuery] string roleName)
        {
            await _userService.AddRoleToUser(idUser, roleName);
            return Ok();
        }
    }
}
