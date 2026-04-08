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
    [Authorize(Roles = "Compliance_Officer")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateAuditResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAudit([FromBody] CreateAuditRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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