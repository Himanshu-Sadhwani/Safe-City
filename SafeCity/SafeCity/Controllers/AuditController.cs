using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.Services.Audit;
using SafeCity.DTOs;
using SafeCity.Utility;

namespace SafeCity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuditController : ControllerBase
{
    private readonly IAuditService _service;
    public AuditController(IAuditService service)
    {
        _service = service;
    }

    /// <summary>
    /// Records a new compliance audit submitted by a Compliance Officer.
    /// </summary>
    /// <param name="request">The audit request containing officer ID, scope, findings, and status.</param>
    /// <returns>
    /// Returns <c>201 Created</c> with the audit details on success;
    /// <c>400 Bad Request</c> if validation fails;
    /// <c>500 Internal Server Error</c> if an unexpected error occurs.
    /// </returns>
    [Authorize(Roles = nameof(UserRoleOption.Compliance_Officer))]
    [HttpPost]
    [ProducesResponseType(typeof(CreateAuditResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAudit([FromBody] CreateAuditRequestDto request)
    {
        try
        {
            var response = await _service.CreateAuditAsync(request);
            return Created($"/api/v1/audit/{response.AuditID}", new { message = "Audit recorded successfully.", data = response });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = ErrorMessages.Audit.InternalError });
        }
    }

    /// <summary>
    /// Retrieves all audit records with optional filters.
    /// </summary>
    /// <param name="scope">Optional filter by audit scope (0=Department, 1=Facility, 2=System, 3=Organization, 4=Incident).</param>
    /// <param name="status">Optional filter by audit status (0=Draft, 1=Finalized, 2=Archived).</param>
    /// <param name="officerId">Optional filter by officer ID.</param>
    /// <param name="sort">Sort order: "asc" for ascending, defaults to descending.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the list of records;
    /// <c>404 Not Found</c> if no records exist;
    /// <c>500 Internal Server Error</c> if an unexpected error occurs.
    /// </returns>
    [Authorize(Roles = nameof(UserRoleOption.Compliance_Officer))]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllAudits(
        [FromQuery] AuditScope? scope,
        [FromQuery] AuditStatus? status,
        [FromQuery] int? officerId,
        [FromQuery] string? sort)
    {
        try
        {
            var response = await _service.GetAllAsync(scope, status, officerId, sort);

            if (response == null || response.Count == 0)
                return NotFound(new { message = "No audit records found." });

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = ErrorMessages.Audit.InternalError });
        }
    }
}