using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
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
    [Authorize(Roles = nameof(UserRoleOption.Compliance_Officer))]
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

    /// <summary>
    /// Retrieves all compliance records with optional filters.
    /// </summary>
    /// <param name="type">Optional filter by compliance type (0 = Incident, 1 = Dispatch).</param>
    /// <param name="result">Optional filter by compliance result (0 = Pass, 1 = Fail).</param>
    /// <param name="sort">Sort order: "asc" for ascending, defaults to descending.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the list of records;
    /// <c>404 Not Found</c> if no records exist;
    /// <c>500 Internal Server Error</c> if an unexpected error occurs.
    /// </returns>
    [Authorize(Roles = nameof(UserRoleOption.Compliance_Officer))]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllCompliance(
        [FromQuery] ComplianceType? type,
        [FromQuery] ComplianceResult? result,
        [FromQuery] string? sort)
    {
        try
        {
            var response = await _service.GetAllAsync(type, result, sort);

            if (response == null || response.Count == 0)
                return NotFound(new { message = "No compliance records found." });

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = ErrorMessages.Compliance.InternalError });
        }
    }
}