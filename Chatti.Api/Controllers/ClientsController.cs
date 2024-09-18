using Chatti.Models;
using Chatti.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService clientsService;

        public ClientsController(ClientsService clientsService)
        {
            this.clientsService = clientsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var list = await clientsService.GetAllAsync();
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ClientRequestModel model)
        {
            var result = await clientsService.CreateAsync(model);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] ClientRequestModel model)
        {
            var result = await clientsService.UpdateAsync(id, model);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await clientsService.DeleteAsync(id);
            return Ok("Item deleted successfully");
        }
    }
}
