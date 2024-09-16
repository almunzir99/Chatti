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
    }
}
