using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.CommenAPi;

namespace FitFlex.Application.Interfaces
{
    public interface IWorkoutPlanService
    {
        Task<APiResponds<WorkoutPlanResponse>> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest requestDto);
        Task<APiResponds<WorkoutPlanResponse>> GetWorkoutPlanByIdAsync(int id);
        Task<APiResponds<IEnumerable<WorkoutPlanResponse>>> GetAllWorkoutPlansAsync();
        Task<APiResponds<WorkoutPlanResponse>> UpdateWorkoutPlanAsync(int id, CreateWorkoutPlanRequest requestDto);
        Task<APiResponds<string>> DeleteWorkoutPlanAsync(int id);
    }
}
