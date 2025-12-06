using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Diet_plan_dtos;
using FitFlex.CommenAPi;

namespace FitFlex.Application.Interfaces
{
    public interface IDietPlanService
    {
        Task<APiResponds<DietPlanResponseDto>> AssignDietPlanAsync(DietPlanRequestDto request, int trainerId);

      
        Task<APiResponds<DietPlanResponseDto>> GetDietPlanByUserIdAsync(int userId);

        Task<APiResponds<DietPlanResponseDto>> UpdateDietPlanAsync(int dietPlanId, DietPlanUpdateDto updateDto);

        
        Task<APiResponds<string>> DeleteDietPlanAsync(int dietPlanId);

        Task<APiResponds<List<DietPlanResponseDto>>> GetDietPlansByTrainerAsync(int trainerId);

        Task<APiResponds<MealResponseDto>> AddMealToDietPlanAsync(int dietPlanId, MealRequestDto mealRequest);

        Task<APiResponds<string>> RemoveMealFromDietPlanAsync(int dietPlanId, int mealId);

    }
}
