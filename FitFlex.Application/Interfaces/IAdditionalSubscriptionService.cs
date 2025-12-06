using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Subscription_model;

namespace FitFlex.Application.Interfaces
{
    public interface IAdditionalSubscriptionService
    {

        Task<APiResponds<AdditionalFeatureResponseDto>> Additonalsubscription(int UserID, AddAdditionalFeatureRequestDto dto);


        Task<APiResponds<SubscriptionPlansResponseDto>> AdditonalsubscriptionByID(int planId);

        Task<APiResponds<List<SubscriptionPlansResponseDto>>> AllAdditionalSubscription();
        Task<APiResponds<bool>> RemoveAdditionalFeatureAsync(int additionalSubscriptionId);
        Task<APiResponds<SubscriptionPlansResponseDto>> CreatePlanAsync(AdditionalSubscriptionPlanDto plan);



    }
}
