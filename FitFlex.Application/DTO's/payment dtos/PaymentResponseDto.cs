using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.payment_dtos
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }                       // Payment Id from DB
        public int UserSubscriptionId { get; set; }      // Linked UserSubscription Id
        public string StripePaymentIntentId { get; set; } // Stripe PaymentIntent Id
        public long Amount { get; set; }                 // Amount in paisa
        public string Currency { get; set; }             // e.g., "inr"
        public string ClientSecret { get; set; }         // Stripe Client Secret for frontend
        public string Status { get; set; }               // "Pending", "Paid", "Failed"
        public DateTime CreatedOn { get; set; }
        public DateTime? PaidOn { get; set; }
    }
}
