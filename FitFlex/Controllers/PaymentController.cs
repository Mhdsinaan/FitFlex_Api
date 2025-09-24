using Microsoft.AspNetCore.Mvc;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.stripePayment;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Infrastructure.Interfaces;
using FitFlex.Application.DTO_s.payment_dtos;

namespace FitFlex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepository<UserSubscription> _userSubscriptionRepo;

        public PaymentController(
            IPaymentService paymentService,
            IRepository<UserSubscription> userSubscriptionRepo)
        {
            _paymentService = paymentService;
            _userSubscriptionRepo = userSubscriptionRepo;
        }


        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent(CreatePaymentIntentRequestDto request)
        {
            try
            {
                var subscription = await _userSubscriptionRepo.GetByIdAsync(request.UserSubscriptionId);
                if (subscription == null)
                    return NotFound("Subscription not found");

                var result = await _paymentService.CreateStripePaymentIntentAsync(subscription);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        public class ConfirmPaymentDto
        {
            public string PaymentIntentId { get; set; }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PaymentIntentId))
                return BadRequest(new { message = "PaymentIntentId is required." });

            var success = await _paymentService.ConfirmPaymentAsync(dto.PaymentIntentId);

            if (!success)
                return BadRequest(new { message = "Payment confirmation failed or already confirmed." });

            return Ok(new { message = "Payment confirmed successfully." });
        }

    }
}
