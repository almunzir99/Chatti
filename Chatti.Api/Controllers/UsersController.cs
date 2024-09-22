using Chatti.Models.Users;
using Chatti.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly UsersService userService;

        public UsersController(UsersService userService)
        {
            this.userService = userService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationModel model)
        {
            var user = await userService.Authenticate(model);
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequestModel model)
        {
            var user = await userService.Register(model);
            return Ok(user);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string? userId = CurrentUserType == "ADMIN" ? null : CurrentUserId;
            var users = await userService.GetUsersListAsync(userId);
            return Ok(users);
        }
    }
}
