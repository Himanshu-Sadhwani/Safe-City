using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Case;
using SafeCity.Services.Case;
using System.Security.Claims;

namespace SafeCity.Controllers
{
    [Route("api/v1/[controller]")]
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
        /// <param name="sort">sort order: asc for ascending or desc for descending</param>
        /// <returns>return a list of Case Reported and verified by the authorities after filteration wherever applicable</returns>
        [Authorize(Roles = "Citizen, Admin")]
        [HttpGet("list-case")]
        public async Task<IActionResult> GetCases([FromQuery] CaseStatusCheck? status, [FromQuery] int? incidentId, [FromQuery] DateTime? resolutionDate, [FromQuery] string? sort)
        {
            try
            {
                // extracting the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);
                // check if the user id  is admin or not
                bool isAdmin = User.IsInRole("Admin");

                var response = await _service.ViewCase(userId, isAdmin, status, incidentId, resolutionDate, sort);
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
        /// <summary>
        /// An api endpoint to create a case for the respective incident id.
        /// </summary>
        /// <param name="request">it will take the CaseDetails parameters as a dto body</param>
        /// <returns>return a success message for case creation and error message when case creation fails</returns>
        [Authorize(Roles = "Emergency_Dispatcher, Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCase(CaseCreation request)
        {
            try
            {
                // validate model state.
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _service.CreateCase(request);
                return Created("", new { message = "Case Created Successfully" });
            }
            catch (Exception ex)
            {

                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { error = msg });

            }
        }
    }
}
