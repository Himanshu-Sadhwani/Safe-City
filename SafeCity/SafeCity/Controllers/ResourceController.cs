using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.Domain.Enum;
using SafeCity.Services.Resource;
using SafeCity.Repository;

namespace SafeCity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        [Authorize(Roles = "Dispatcher, Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ViewResources(
            [FromQuery] int? resourceId,
            [FromQuery] ResourceTypeOption? type,
            [FromQuery] ResourceAvailabilityOption? availability,
            [FromQuery] string? location,
            [FromQuery] string? sortOrder)
        {
            try
            {
                var response = await _resourceService.ViewResources(
                    resourceId,
                    type,
                    availability,
                    location,
                    sortOrder
                );

                if (response == null || response.Count == 0)
                    return NotFound("No resources found.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("exists")]
        public async Task<IActionResult> UnitExists([FromQuery] string unitName)
        {
            if (string.IsNullOrWhiteSpace(unitName))
                return BadRequest(new { message = "Unit name is required." });

            var exists = await _resourceService.UnitExistsAsync(unitName);
            return Ok(new { exists });
        }
    }
}