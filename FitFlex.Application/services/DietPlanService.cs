using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Diet_plan_dtos;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Meal;
using FitFlex.Domain.Entities.NewFolder;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.services
{
    public class DietPlanService : IDietPlanService
    {
        private readonly IRepository<UserDietPlan> _userDietRepo;
        private readonly IRepository<Meal> _mealRepo;
        private readonly IRepository<Trainer> _trainerrepo;
        private readonly IRepository<UserTrainer> _usertrainer;
        public DietPlanService(IRepository<UserDietPlan> dietplanrepo, IRepository<Meal> mealRepo, IRepository<Trainer> trainerrepo, IRepository<UserTrainer> usertrainer)
        {
            _userDietRepo = dietplanrepo;
            _mealRepo=mealRepo;
            _trainerrepo = trainerrepo;
            _usertrainer = usertrainer;
        }
        public async Task<APiResponds<MealResponseDto>> AddMealToDietPlanAsync(int dietPlanId, MealRequestDto mealRequest)
        {
            try
            {
                var dietPlan = await _userDietRepo.GetByIdAsync(dietPlanId);
                if (dietPlan == null)
                    return new APiResponds<MealResponseDto>("404", "Diet plan not found", null);

                var meal = new Meal
                {
                    MealType = mealRequest.MealType,
                    FoodItems = mealRequest.FoodItems,
                    Calories = mealRequest.Calories
                };

                await _mealRepo.AddAsync(meal);
                await _mealRepo.SaveChangesAsync();

                

                 _userDietRepo.Update(dietPlan);

                var response = new MealResponseDto
                {
                    Id = meal.Id,
                    MealType = meal.MealType,
                    FoodItems = meal.FoodItems,
                    Calories = meal.Calories
                };

                return new APiResponds<MealResponseDto>("200", "Meal added successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<MealResponseDto>("500", ex.Message, null);
            }
        }


        public async Task<APiResponds<DietPlanResponseDto>> AssignDietPlanAsync(DietPlanRequestDto request, int trainerId)
        {
            try
            {
                var trainer = await _usertrainer.GetAllAsync();
                var exist = trainer.FirstOrDefault(p => p.UserId == request.UserId && p.TrainerId == trainerId);
                if(exist is null)
                    return new APiResponds<DietPlanResponseDto>("404", "Trainer notdvasdajhdadk found", null);

                var existingPlan = (await _userDietRepo.GetAllAsync())
                    .FirstOrDefault(p => p.UserId == request.UserId && p.StartDate.Month == request.StartDate.Month);
                if (existingPlan != null)
                    return new APiResponds<DietPlanResponseDto>("400", "User already has a diet plan this month", null);

                var dietPlan = new UserDietPlan
                {
                    UserId = request.UserId,
                    TrainerId = trainerId,
                    PlanName = request.PlanName,
                    Description = request.Description,
                    Calories = request.Calories,
                    StartDate = request.StartDate,
                    EndDate = request.StartDate.AddMonths(1).AddDays(-1)
                };

                await _userDietRepo.AddAsync(dietPlan);
                await _userDietRepo.SaveChangesAsync();

                var meals = new List<Meal>
        {
            new Meal { DietPlanId = dietPlan.Id, MealType = "Breakfast", FoodItems = request.Breakfast ?? "", Calories = request.BreakfastCalories },
            new Meal { DietPlanId = dietPlan.Id, MealType = "MidMeal", FoodItems = request.MidMeal ?? "", Calories = request.MidMealCalories },
            new Meal { DietPlanId = dietPlan.Id, MealType = "Lunch", FoodItems = request.Lunch ?? "", Calories = request.LunchCalories },
            new Meal { DietPlanId = dietPlan.Id, MealType = "EveningSnack", FoodItems = request.EveningSnack ?? "", Calories = request.EveningSnackCalories },
            new Meal { DietPlanId = dietPlan.Id, MealType = "Dinner", FoodItems = request.Dinner ?? "", Calories = request.DinnerCalories }
        };


                foreach (var meal in meals)
                {
                    await _mealRepo.AddAsync(meal);
                    await _mealRepo.SaveChangesAsync();
                }

               
                var response = new DietPlanResponseDto
                {
                    Id = dietPlan.Id,
                    UserId = dietPlan.UserId,
                    TrainerId = dietPlan.TrainerId,
                    PlanName = dietPlan.PlanName,
                    Description = dietPlan.Description,
                    Calories = dietPlan.Calories,
                    StartDate = dietPlan.StartDate,
                    EndDate = dietPlan.EndDate,
                    Meals = meals.Select(m => new MealResponseDto
                    {
                        Id = m.Id,
                        MealType = m.MealType,
                        FoodItems = m.FoodItems,
                        Calories = m.Calories
                    }).ToList()
                };

                return new APiResponds<DietPlanResponseDto>("200", "Diet plan assigned successfully", response);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new APiResponds<DietPlanResponseDto>("500", $"Error: {errorMessage}", null);
            }
        }


        public async Task<APiResponds<string>> DeleteDietPlanAsync(int dietPlanId)
        {
            try
            {
                var dietPlan = await _userDietRepo.GetByIdAsync(dietPlanId);
                if (dietPlan == null)
                {
                    return new APiResponds<string>("404", "Diet plan not found", null);
                }

          
                dietPlan.IsDelete = true;
                _userDietRepo.Update(dietPlan);

                await _userDietRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "Diet plan deleted successfully", "Deleted");
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<DietPlanResponseDto>> GetDietPlanByUserIdAsync(int userId)
        {
            try
            {
                // Fetch the diet plan for the user
                var userDietPlan = await _userDietRepo.GetByIdAsync(userId);
                if (userDietPlan == null)
                    return new APiResponds<DietPlanResponseDto>("404", "DietPlan by UserId not found", null);

                // Map entity to DTO
                var response = new DietPlanResponseDto
                {
                    Id = userDietPlan.Id,
                    UserId = userDietPlan.UserId,
                    TrainerId = userDietPlan.TrainerId,
                    PlanName = userDietPlan.PlanName,
                    Description = userDietPlan.Description,
                    Calories = userDietPlan.Calories,
                    StartDate = userDietPlan.StartDate,
                    EndDate = userDietPlan.EndDate,
                    Meals =  new List<MealResponseDto>()
                };

                return new APiResponds<DietPlanResponseDto>("200", "DietPlan fetched successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<DietPlanResponseDto>("500", $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<List<DietPlanResponseDto>>> GetDietPlansByTrainerAsync(int trainerId)
        {
            try
            {
                // Fetch all diet plans for the given trainer
                var dietPlans = await _userDietRepo.GetAllAsync();
                var trainerPlans = dietPlans.Where(dp => dp.TrainerId == trainerId).ToList();

                if (!trainerPlans.Any())
                    return new APiResponds<List<DietPlanResponseDto>>("404", "No diet plans found for this trainer", null);

                // Map entities to DTOs
                var response = trainerPlans.Select(dp => new DietPlanResponseDto
                {
                    Id = dp.Id,
                    UserId = dp.UserId,
                    TrainerId = dp.TrainerId,
                    PlanName = dp.PlanName,
                    Description = dp.Description,
                    Calories = dp.Calories,
                    StartDate = dp.StartDate,
                    EndDate = dp.EndDate,
                    Meals = new List<MealResponseDto>() // empty list
                }).ToList();

                return new APiResponds<List<DietPlanResponseDto>>("200", "Diet plans fetched successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<DietPlanResponseDto>>("500", $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<string>> RemoveMealFromDietPlanAsync(int dietPlanId, int mealId)
        {
            try
            {
                // Fetch the meal by ID and check if it belongs to the given diet plan
                var meal = await _mealRepo.GetByIdAsync(mealId);
                if (meal == null || meal.DietPlanId != dietPlanId)
                    return new APiResponds<string>("404", "Meal not found in this diet plan", null);

                // Remove the meal
                _mealRepo.Delete(meal);
                await _mealRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "Meal removed from diet plan successfully", "Removed");
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<DietPlanResponseDto>> UpdateDietPlanAsync(int dietPlanId, DietPlanUpdateDto updateDto)
        {
            try
            {
                // Fetch the diet plan from the repository
                var dietPlan = await _userDietRepo.GetByIdAsync(dietPlanId);
                if (dietPlan == null)
                    return new APiResponds<DietPlanResponseDto>("404", "Diet plan not found", null);

                // Update fields from the DTO
                dietPlan.PlanName = updateDto.PlanName ?? dietPlan.PlanName;
                dietPlan.Description = updateDto.Description ?? dietPlan.Description;
                dietPlan.Calories = updateDto.Calories ?? dietPlan.Calories;
                dietPlan.StartDate = updateDto.StartDate ?? dietPlan.StartDate;
                dietPlan.EndDate = updateDto.EndDate ?? dietPlan.EndDate;

                // Save changes
                 _userDietRepo.Update(dietPlan);

                // Map updated entity to response DTO
                var response = new DietPlanResponseDto
                {
                    Id = dietPlan.Id,
                    UserId = dietPlan.UserId,
                    TrainerId = dietPlan.TrainerId,
                    PlanName = dietPlan.PlanName,
                    Description = dietPlan.Description,
                    Calories = dietPlan.Calories,
                    StartDate = dietPlan.StartDate,
                    EndDate = dietPlan.EndDate,
                    Meals = new List<MealResponseDto>() // Empty since meals not in entity
                };

                return new APiResponds<DietPlanResponseDto>("200", "Diet plan updated successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<DietPlanResponseDto>("500", $"An error occurred: {ex.Message}", null);
            }
        }

    }
}
