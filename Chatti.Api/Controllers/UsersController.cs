﻿using Chatti.Models.Users;
using Chatti.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
            Request.Headers.TryGetValue("TenantId", out var tenantId);
            var user = await userService.Authenticate(tenantId, model);
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequestModel model)
        {
            Request.Headers.TryGetValue("TenantId", out var tenantId);
            var user = await userService.Register(tenantId, model);
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
        [Authorize(Roles = "ADMIN")]
        [HttpGet("{userId}/password-reset")]
        public async Task<IActionResult> ResetPasswordAsync([FromRoute] string userId, [Required][FromQuery] string password)
        {
            await userService.ResetPassword(userId,password);
            return Ok("password has been resetted successfully");
        }
    }
}
