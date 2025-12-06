
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.Interfaces
{
    public interface ISubscription
    {

        Task<APiResponds<IEnumerable<SubscriptionPlansResponseDto>>> GetAllPlansAsync();
        Task<APiResponds<SubscriptionPlansResponseDto>> GetPlanByIdAsync(int id);
        Task<APiResponds<SubscriptionPlansResponseDto>> CreatePlanAsync(SubscriptionPlanDto plan);
        
        Task<APiResponds<SubscriptionPlansResponseDto>> UpdatePlanAsync(int id, SubscriptionPlanDto planDto);
        Task<APiResponds<bool>> DeletePlanAsync(int id);


    }


}
