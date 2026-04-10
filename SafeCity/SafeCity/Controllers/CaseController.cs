using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.Services.Case;
using System.Security.Claims;

namespace SafeCity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _service;
        public CaseController(ICaseService service)
        {
            _service = service;
        }

        /// <summary>
        /// View Case Api that will return the list of case made by the citizen with filter and without filter based on the filter we passed 
        /// </summary>
        /// <param name="status">it will check Case Status enum and will apply filteration on it</param>
        /// <param name="incidentId">based on the incident id we can fetch data</param>
        /// <param name="resolutionDate">based on the resolution date </param>
        /// <returns>return a list of Case Reported and verified by the authorities after filteration wherever applicable</returns>
        [Authorize(Roles = "Citizen, Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ViewCase([FromQuery] CaseStatusCheck? status, [FromQuery] int? incidentId, [FromQuery] DateTime? resolutionDate)
        {
            try
            {
                // extracting the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);
                // check if the user id  is admin or not
                bool isAdmin = User.IsInRole("Admin");

                var response = await _service.ViewCase(userId, isAdmin, status, incidentId, resolutionDate);
                // if no case is found then
                if (response == null || response.Count == 0)
                {
                    return NotFound(new { message = "No cases found." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // throws errror if any present
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
