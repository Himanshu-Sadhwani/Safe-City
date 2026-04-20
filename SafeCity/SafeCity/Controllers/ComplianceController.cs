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

    [Authorize(Roles = "Compliance_Officer")]
    [HttpPost]
    public async Task<IActionResult> CreateCompliance([FromBody] CreateComplianceRequestDto request)
    {
        try
        {
            var response = await _service.CreateComplianceAsync(request);
            return Ok(new { message = "Compliance recorded successfully.", data = response });
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