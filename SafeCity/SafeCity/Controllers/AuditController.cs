using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "Compliance_Officer")]
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
} 