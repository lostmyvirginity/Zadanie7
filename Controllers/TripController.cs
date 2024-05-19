using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models.DTOs;
using Zadanie7.Services;


namespace Zadanie7.Controllers
{
    [ApiController]
    [Route("api")]
    public class TripsController : ControllerBase  
    {
        private readonly ITripService _service;

        public TripsController(ITripService tripService)
        {
            _service = tripService ?? throw new ArgumentNullException(nameof(tripService));
        }

        [HttpGet("trips")]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            var trips = await _service.GetTrips();
            if (!trips.Any())
            {
                return NotFound("No trips found");
            }
            return Ok(trips);
        }

        [HttpDelete("clients/{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _service.DeleteClient(id);
            if (!result)
            {
                return NotFound($"Client with ID {id} not found.");
            }
            return Ok($"Client with ID {id} was deleted.");
        }
        [HttpPost("{idTrip:int}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] InputClientDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.AddClientToTripAsync(idTrip, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}