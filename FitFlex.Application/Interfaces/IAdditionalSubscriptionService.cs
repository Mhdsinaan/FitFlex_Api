using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.CommenAPi;

namespace FitFlex.Application.Interfaces
{
    public interface IAdditionalSubscriptionService
    {
        Task<APiResponds<AdditionalFeatureResponseDto>> AddAdditionalFeatureAsync(AddAdditionalFeatureRequestDto request);

        Task<APiResponds<List<AdditionalFeatureResponseDto>>> GetAddOnsByUserSubscriptionAsync(int userSubscriptionId);

        Task<APiResponds<bool>> RemoveAdditionalFeatureAsync(int additionalSubscriptionId);
    }
}
