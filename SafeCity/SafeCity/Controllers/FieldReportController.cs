using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.FieldReport;
using SafeCity.Services.FieldReport;
using SafeCity.Utility;
using System.Security.Claims;

namespace SafeCity.Controllers
{
    /// <summary>
    /// Exposes field report endpoints. Accessible by Police officers and Admins only.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRoleOption.Police) + ", " + nameof(UserRoleOption.Admin))]
    public class FieldReportController : ControllerBase
    {
        private readonly IFieldReportService _fieldReportService;

        /// <summary>
        /// Initializes a new instance of <see cref="FieldReportController"/>.
        /// </summary>
        public FieldReportController(IFieldReportService fieldReportService)
        {
            _fieldReportService = fieldReportService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFieldReports([FromQuery] FieldReportFilterDto filter)
        {
            try
            {
                var result = await _fieldReportService.GetAllAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Submits a new field activity report for the authenticated officer's patrol.
        /// Returns 201 Created, 400 Bad Request, 403 Forbidden, 404 Not Found, or 409 Conflict.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateFieldReport([FromBody] CreateFieldReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Please enter all the required fields" });

            int officerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var response = await _fieldReportService.CreateAsync(dto, officerId);
                return Created(string.Empty, response);;
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Partially updates the notes and/or status of an existing field report.
        /// Only the authenticated officer who owns the report may update it.
        /// Returns 200 OK, 400 Bad Request, 403 Forbidden, or 404 Not Found.
        /// </summary>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateFieldReport(int id, [FromBody] UpdateFieldReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int officerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var response = await _fieldReportService.UpdateAsync(id, dto, officerId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
