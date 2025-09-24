using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitFlex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscription _subscriptionService;

        public SubscriptionController(ISubscription subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlans()
        {
            var result = await _subscriptionService.GetAllPlansAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(int id)
        {
            var result = await _subscriptionService.GetPlanByIdAsync(id);
            if (result == null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePlan([FromBody] SubscriptionPlanDto dto)
        {
            var result = await _subscriptionService.CreatePlanAsync(dto);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] SubscriptionPlanDto dto)
        {
            var result = await _subscriptionService.UpdatePlanAsync(id, dto);
            if (result == null) return NotFound(result);
            return Ok(result);
        }

        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var result = await _subscriptionService.DeletePlanAsync(id);
            if (result == null || !result.Data)
                return NotFound(result);
            return Ok(result);
        }
    }
}
