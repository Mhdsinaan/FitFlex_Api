using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace FitFlex.Controllers
{
    public class AdditionalSubscriptionController : ControllerBase
    {
        private readonly IAdditionalSubscriptionService _service;
        public AdditionalSubscriptionController(IAdditionalSubscriptionService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddAdditionalFeatureRequestDto request)
        {
            var result = await _service.AddAdditionalFeatureAsync(request);
            return Ok(result);
        }

        [HttpGet("subscription/{userSubscriptionId}")]
        public async Task<IActionResult> GetBySubscription(int userSubscriptionId)
        {
            var result = await _service.GetAddOnsByUserSubscriptionAsync(userSubscriptionId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.RemoveAdditionalFeatureAsync(id);
            return Ok(result);
        }
    }
}

