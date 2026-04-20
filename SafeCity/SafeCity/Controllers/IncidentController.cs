using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Incidents;
using SafeCity.Services.IncidentService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SafeCity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        // dependency injection
        private readonly IIncidentService _incidentService;
        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        /// <summary>
        /// Submits a new incident report on behalf of the authenticated citizen.
        /// </summary>
        /// <remarks>This action requires the user to be authenticated as a citizen. The citizen ID is
        /// automatically set based on the authenticated user's identity and does not need to be provided in the request
        /// body.</remarks>
        /// <param name="incidentCreateRequest">An object containing the details of the incident to be reported. Must include all required incident
        /// information; the citizen ID is populated from the authenticated user.</param>
        /// <returns>A response indicating the result of the submission. Returns a 201 Created result with a success message if
        /// the incident is submitted successfully; otherwise, returns a 400 Bad Request for validation errors or a 500
        /// Internal Server Error for unexpected failures.</returns>
        [Authorize(Roles = "Citizen")]
        [HttpPost]
        public async Task<IActionResult> SubmitIncident(IncidentCreateRequest incidentCreateRequest)
        {
            try
            {
                // Extract the UserId from the Token present in the Headers.
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Parse the UserId in integer and stores it
                int userId = int.Parse(userIdClaim);

                // validate the model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // populate the citizen id from the token 
                incidentCreateRequest.CitizenID = userId;
                await _incidentService.SubmitIncident(incidentCreateRequest);

                // If a Incident is submitted successfully
                return Created("", new { message = "Incident Submitted Succesfully" });

            }
            catch (ValidationException ex)
            {
                // if some field validation failes
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Returns a 400 Bad Request with your specific message
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }

        /// ViewIncident Api that takes incidentStatusOption, Location, Type, and Date from query parameters.
        /// it will apply the filter based on each filter type we give in the parameter
        /// </summary>
        /// <param name="status">Take the IncidentStatus Option like pending , inprogess and resolved and it will apply the filter</param>
        /// <param name="location">Take the location and apply filtered based on the location</param>
        /// <param name="type">Take the type of incident like crime ,fire, Accident</param>
        /// <param name="date"></param>
        /// <returns>returns the Filtered List based on the Roles and the Filters we Applied</returns>
        [Authorize(Roles = "Citizen, Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ViewIncident(
            [FromQuery] IncidentStatusOption? status,
            [FromQuery] string? location,
            [FromQuery] IncidentOption? type,
            [FromQuery] DateTime? date)
        {
            try
            {
                // Extracting the user information from the token
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                bool isAdmin = User.IsInRole("Admin");

                // Calling the service layer 
                var response = await _incidentService.ViewIncident(userId, isAdmin, status, location, type, date);

                if (response == null || response.Count == 0)
                {
                    return NotFound("No incidents found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // throws errors if any present while handling the request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
