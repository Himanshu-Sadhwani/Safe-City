using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.Incidents;
using SafeCity.Services.IncidentService;
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
            catch (ArgumentException ex)
            {
                // if some field validation failes
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// ViewIncident Api that takes incidentStatusOption from the query parameters and 
        /// Extracts the user Information from the token and based on the token details it will return the list of Incident with filterable condition like Pending, InProgress, Resolved.
        /// </summary>
        /// <param name="incidentStatusOption">incidentStatusOption from the query parameters in integer form like 0,1,2</param>
        /// <returns>returns a list of incident made by the citizen</returns>
        [Authorize(Roles = "Citizen, Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ViewIncident([FromQuery] int incidentStatusOption = 0)
        {
            try
            {
                // Extracting the user information from the token
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                bool isAdmin = User.IsInRole("Admin");
                var response = await _incidentService.ViewIncident(userId, isAdmin, incidentStatusOption);

                if (response == null)
                {
                    return NotFound("No incidents found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // it will throw errors if any error is present
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
