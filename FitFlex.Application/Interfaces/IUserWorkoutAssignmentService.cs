using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.CommenAPi;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.Interfaces
{
    public interface IUserWorkoutAssignmentService
    {
        Task<APiResponds<AssignWorkoutResponseDto>> AssignWorkoutAsync(AssignWorkoutRequest request, int TrainerId);

        Task<APiResponds<List<UserWorkoutAssignmentResponse>>> GetTodayWorkoutsByUserAsync(int userId);

        Task<APiResponds<List<WorkoutDto>>> GetAllWorkoutsByUserAsync(int userId);
        Task<APiResponds<WorkoutDto>> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus status);

        Task<APiResponds<string>> DeleteAssignmentAsync(int assignmentId);
    }
}
