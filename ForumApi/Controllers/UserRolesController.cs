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
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userService;

        public UserRolesController(IUserRolesService userService)
        {
            _userService = userService;
        }

        [HttpPost("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddRoleToUser([FromRoute] int userId, [FromQuery] string roleName)
        {
            await _userService.AddRoleToUser(userId, roleName);
            return Ok();
        }

        [HttpDelete("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveRoleFromUser([FromRoute] int userId, [FromQuery] string roleName)
        {
            await _userService.RemoveRoleFromUser(userId, roleName);
            return NoContent();
        }

        [HttpGet("{userId}/roles")]
        public async Task<ActionResult> GetUserRoles([FromRoute] int userId)
        {
            var roles = await _userService.GetUserRoles(userId);
            return Ok(roles);
        }
    }
}
