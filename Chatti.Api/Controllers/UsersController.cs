using Chatti.Models.Users;
using Chatti.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
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
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequest model)
        {
            var user = await userService.Register(model);
            return Ok(user);
        }
    }
}
