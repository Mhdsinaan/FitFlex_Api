using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.CommenAPi;

namespace FitFlex.Application.Interfaces
{
    public interface IUserSubscription
    {
        Task<APiResponds<UserTrainerResponseDto>> TrainerSelcetion(TrainerSelectingDtoTrainerSelectingDto dto);
        Task<APiResponds<UserSubscriptionResponseDto>> SubscriptionSelection(SubscriptionSelectionDto dto);
        Task<APiResponds<List<UserSubscriptionResponseDto>>> AllUserSubscriptions();

        Task<APiResponds<UserSubscriptionResponseDto>> GetUserSubscriptionByUserId(int userId);
        Task<APiResponds<List<UserSubscriptionResponseDto>>> GetSubscriptionsByTrainerId(int trainerId);

        Task<APiResponds<bool>> BlockSubscriptionAsync(int userId);
        Task<APiResponds<bool>> UnblockSubscriptionAsync(int userId);
        //Task<APiResponds<bool>> DeleteIfExpiredAsync(int subscriptionId);


    }
}
