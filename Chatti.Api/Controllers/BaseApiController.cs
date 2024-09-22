using Chatti.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected string CurrentUserId
        {
            get
            {
                string currentUserId = HttpContext.User.GetClaimValue("id");
                return currentUserId;
            }
        }

        protected string CurrentUserType
        {
            get
            {
                string type = HttpContext.User.GetClaimValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                return type;
            }
        }
    }
}
