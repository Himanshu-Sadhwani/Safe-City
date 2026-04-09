using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services.Dispatch;

namespace SafeCity.Controllers
{
    /// <summary>
    /// API controller responsible for dispatch-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchService _dispatchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchController"/> class.
        /// </summary>
        /// <param name="dispatchService">
        /// The dispatch service that handles business logic for unit assignment.
        /// </param>
        public DispatchController(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
        }

        /// <summary>
        /// Assigns an available resource unit to an incident. </summary>
        /// <param name="request">
        /// The dispatch request containing incident and dispatcher information.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with dispatch details if successful,
        /// or <see cref="BadRequestObjectResult"/> if validation or processing fails.</returns>
        /// <response code="200">Resource successfully assigned to the incident.</response>
        /// <response code="400">Invalid request data or assignment failure.</response>
        [Authorize(Roles = "Emergency_Dispatcher , Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUnit([FromBody] DispatchRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _dispatchService.AssignUnitAsync(request);
                return Ok(new
                {
                    message = "Successfully Dispatched Resource",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}