using System;
using System.Collections.Generic;

namespace FitFlex.Application.DTO_s.payment_dtos
{
    public class CreatePaymentIntentRequestDto
    {
        public int UserSubscriptionId { get; set; }

        // List of AddOn IDs to include in the payment
        public List<int> AddOnIds { get; set; } = new List<int>();
    }
}
