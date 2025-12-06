using Microsoft.AspNetCore.Mvc;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Infrastructure.Interfaces;
using FitFlex.Application.DTO_s.payment_dtos;
using FitFlex.CommenAPi;

namespace FitFlex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepository<UserSubscription> _userSubscriptionRepo;
        private readonly IRepository<UserSubscriptionAddOn> _userAddOnRepo;

        public PaymentController(
            IPaymentService paymentService,
            IRepository<UserSubscription> userSubscriptionRepo,
            IRepository<UserSubscriptionAddOn> userAddOnRepo)
        {
            _paymentService = paymentService;
            _userSubscriptionRepo = userSubscriptionRepo;
            _userAddOnRepo = userAddOnRepo;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequestDto request)
        {
            var mainSubscription = await _userSubscriptionRepo.GetByIdAsync(request.UserSubscriptionId);
            if (mainSubscription == null)
                return Ok(new APiResponds<PaymentResponseDto>("404", "Main subscription not found", null));

            // Fetch add-ons
            var addOns = (await _userAddOnRepo.GetAllAsync())
                         .Where(a => request.AddOnIds.Contains(a.Id))
                         .ToList();

            var result = await _paymentService.CreateStripePaymentIntentForMultipleAsync(mainSubscription, addOns);
            return Ok(result);
        }

        public class ConfirmPaymentDto
        {
            public string PaymentIntentId { get; set; }
            public List<int> AddOnIds { get; set; }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PaymentIntentId))
                return Ok(new APiResponds<bool>("400", "PaymentIntentId is required", false));

            // Fetch add-ons
            var addOns = (await _userAddOnRepo.GetAllAsync())
                         .Where(a => dto.AddOnIds.Contains(a.Id))
                         .ToList();

            var result = await _paymentService.ConfirmPaymentForMultipleAsync(dto.PaymentIntentId, addOns);
            return Ok(result);
        }
    }
}
