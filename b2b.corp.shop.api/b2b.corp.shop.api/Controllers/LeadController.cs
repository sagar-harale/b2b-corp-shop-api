using b2b.corp.shop.api.Service;
using b2b.corp.shop.dbmodel.Models;
using Microsoft.AspNetCore.Mvc;

namespace b2b.corp.shop.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLead([FromQuery] Guid id)
        {
            var response = await _leadService.GetLeadAsync(id);
            return StatusCode((int)response.Context.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeads()
        {
            var response = await _leadService.GetAllLeadsAsync();
            return StatusCode((int)response.Context.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] Lead lead)
        {
            var response = await _leadService.CreateLeadAsync(lead);
            return StatusCode((int)response.Context.StatusCode, response);
        }
    }
}
