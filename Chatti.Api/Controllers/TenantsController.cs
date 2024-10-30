using Chatti.Models.Tenants;
using Chatti.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly TenantsService tenantsService;

        public TenantsController(TenantsService tenantService)
        {
            this.tenantsService = tenantService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var list = await tenantsService.GetAllAsync();
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TenantRequestModel model)
        {
            var result = await tenantsService.CreateAsync(model);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] TenantRequestModel model)
        {
            var result = await tenantsService.UpdateAsync(id, model);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await tenantsService.DeleteAsync(id);
            return Ok("Item deleted successfully");
        }
    }
}
