using System.Threading.Tasks;
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

        [HttpPost("new_Creation")]
   
        public async Task<IActionResult> AdditionaPlanation([FromBody] AdditionalSubscriptionPlanDto dto)
        {
            var create = await _service.CreatePlanAsync(dto);
            if (create is null) return NotFound(create);
            return Ok(create);

        }
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseSubscription([FromBody] AddAdditionalFeatureRequestDto dto)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var result = await _service.Additonalsubscription(userId,dto);
            if (result.StatusCode != "200")
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAdditionalSubscription()
        {
            var all = await _service.AllAdditionalSubscription();
            if (all is null) return NotFound(all);
            return Ok(all);
        }

        [HttpGet("subscription/{userSubscriptionId}")]
        public async Task<IActionResult> GetBySubscription(int userSubscriptionId)
        {
            var result = await _service.AdditonalsubscriptionByID(userSubscriptionId);
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

