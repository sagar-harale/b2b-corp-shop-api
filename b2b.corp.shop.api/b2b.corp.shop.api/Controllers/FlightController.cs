using b2b.corp.shop.api.Models.Api;
using b2b.corp.shop.api.Models.Common;
using b2b.corp.shop.api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace b2b.corp.shop.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly AmadeusFlightService _amadeusFlightService;

        public FlightController(AmadeusFlightService amadeusFlightService)
        {
            _amadeusFlightService = amadeusFlightService;
        }

        [HttpPost("flightlisting")]
        public async Task<IActionResult> FlightListing([FromBody] FlightListingApiRequest listing)
        {
            if (listing == null)
                return BadRequest("Request body is required.");

            var apiResponse = await _amadeusFlightService.GetFlightOffersAsync(listing);
            if (apiResponse.Context.StatusCode == 200)
                return Ok(apiResponse);
            else if (apiResponse.Context.StatusCode == 400)
                return BadRequest(apiResponse);
            else
                return StatusCode((int)apiResponse.Context.StatusCode, apiResponse);
        }
    }
}
