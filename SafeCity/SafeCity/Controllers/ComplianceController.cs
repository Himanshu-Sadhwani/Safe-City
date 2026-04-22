using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs;
using SafeCity.Services.Compliance;
using SafeCity.Utility;

namespace SafeCity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ComplianceController : ControllerBase
{
    private readonly IComplianceService _service;
    public ComplianceController(IComplianceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Records a new compliance entry submitted by a Compliance Officer.
    /// </summary>
    /// <param name="request">The compliance request containing entity ID, type, result, and notes.</param>
    /// <returns>
    /// Returns <c>201 Created</c> with a success message on success;
    /// <c>400 Bad Request</c> if validation fails;
    /// <c>500 Internal Server Error</c> if an unexpected error occurs.
    /// </returns>
    [Authorize(Roles = "Compliance_Officer")]
    [HttpPost]
    public async Task<IActionResult> CreateCompliance([FromBody] CreateComplianceRequestDto request)
    {
        try
        {
            await _service.CreateComplianceAsync(request);
            return StatusCode(201, new { message = "Compliance recorded successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = ErrorMessages.Compliance.InternalError });
        }
    }
}