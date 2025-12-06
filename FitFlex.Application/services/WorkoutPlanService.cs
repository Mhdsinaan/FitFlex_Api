using System.Security.Claims;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Entities.WorkoutPlan_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FitFlex.Application.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IRepository<WorkoutPlan> _workoutPlanRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Trainer> _trainerRepo;
        public WorkoutPlanService(IRepository<WorkoutPlan> workoutPlanRepo, IHttpContextAccessor httpContextAccessor, IRepository<Trainer> trainerRepo)
        {
            _workoutPlanRepo = workoutPlanRepo;
            _httpContextAccessor = httpContextAccessor;
            _trainerRepo = trainerRepo;

        }

        public async Task<APiResponds<WorkoutPlanResponse>> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest requestDto)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = string.IsNullOrEmpty(userIdClaim) ? 0 : int.Parse(userIdClaim);
                if(userId==null) return new APiResponds<WorkoutPlanResponse>("401", "please login", null);

               

                var plan = new WorkoutPlan
                {
                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    Level = requestDto.Level,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = userId,  
                    Exercises = requestDto.Exercises?.Select(e => new WorkoutExercise
                    {
                        ExerciseName = e.ExerciseName,
                        Sets = e.Sets,
                        Reps = e.Reps,
                    }).ToList() ?? new List<WorkoutExercise>()
                };


                await _workoutPlanRepo.AddAsync(plan);
                await _workoutPlanRepo.SaveChangesAsync();

                return new APiResponds<WorkoutPlanResponse>("200", "Workout plan created successfully", MapToResponse(plan));
            }
            catch (Exception ex)
            {
                return new APiResponds<WorkoutPlanResponse>("500", ex.Message, null);
            }
        }


        public async Task<APiResponds<WorkoutPlanResponse>> GetWorkoutPlanByIdAsync(int id)
        {
            try
            {
                var plan = await _workoutPlanRepo.GetAllQueryable()
                    .Include(p => p.Exercises)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (plan == null)
                    return new APiResponds<WorkoutPlanResponse>("404", "Workout plan not found", null);

                return new APiResponds<WorkoutPlanResponse>("200", "Workout plan fetched successfully", MapToResponse(plan));
            }
            catch (Exception ex)
            {
                return new APiResponds<WorkoutPlanResponse>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<IEnumerable<WorkoutPlanResponse>>> GetAllWorkoutPlansAsync()
        {
            try
            {
                var plans = await _workoutPlanRepo.GetAllQueryable()
                    .Include(p => p.Exercises)
                    .ToListAsync();

                var response = plans.Select(MapToResponse);

                return new APiResponds<IEnumerable<WorkoutPlanResponse>>("200", "Workout plans fetched successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<IEnumerable<WorkoutPlanResponse>>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<WorkoutPlanResponse>> UpdateWorkoutPlanAsync(int id, CreateWorkoutPlanRequest requestDto)
        {
            try
            {
                var plan = await _workoutPlanRepo.GetByIdAsync(id);
                if (plan == null)
                    return new APiResponds<WorkoutPlanResponse>("404", "Workout plan not found", null);

                plan.Name = requestDto.Name;
                plan.Description = requestDto.Description;
                plan.Level = requestDto.Level;

               

                _workoutPlanRepo.Update(plan);
                await _workoutPlanRepo.SaveChangesAsync();

                return new APiResponds<WorkoutPlanResponse>("200", "Workout plan updated successfully", MapToResponse(plan));
            }
            catch (Exception ex)
            {
                return new APiResponds<WorkoutPlanResponse>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<string>> DeleteWorkoutPlanAsync(int id)
        {
            try
            {
                var plan = await _workoutPlanRepo.GetByIdAsync(id);
                if (plan == null)
                    return new APiResponds<string>("404", "Workout plan not found", null);

                _workoutPlanRepo.Delete(plan);
                await _workoutPlanRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "Workout plan deleted successfully", null);
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", ex.Message, null);
            }
        }

       
        private WorkoutPlanResponse MapToResponse(WorkoutPlan plan)
        {
            return new WorkoutPlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Level = plan.Level.ToString(),
                TrainerID = plan.CreatedBy,
               
                Exercises = plan.Exercises?.Select(e => new WorkoutExerciseResponse
                {
                    Id = e.Id,
                    ExerciseName = e.ExerciseName,
                    Sets = e.Sets,
                    Reps = e.Reps,
                   
                }).ToList() ?? new List<WorkoutExerciseResponse>()
            };
        }
    }
}
