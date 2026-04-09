using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCity.DTOs.CrisisDtos;
using SafeCity.Services.Crisis;
using System;
using System.Threading.Tasks;
 
namespace SafeCity.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    public class CrisisController : ControllerBase
    {
        private readonly ICrisisService _crisisService;
 
        public CrisisController(ICrisisService crisisService)
        {
            _crisisService = crisisService;
        }
 
        [HttpPost]
        public async Task<IActionResult> DeclareCrisis([FromBody] CreateCrisisRequestDto request)
        {
            try
            {
                var result = await _crisisService.DeclareCrisis(request);
                return StatusCode(201,result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}