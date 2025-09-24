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
        Task<APiResponds<string>> AssignWorkoutPlanAsync(int userId, int workoutPlanId);
        Task<APiResponds<string>> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus newStatus);
        Task<APiResponds<UserWorkoutAssignmentResponse>> GetAssignmentByIdAsync(int id);
        Task<APiResponds<IEnumerable<UserWorkoutAssignmentResponse>>> GetAssignmentsByUserAsync(int userId);
        Task<APiResponds<List<UserWorkoutAssignmentResponse>>> GetAllAssignmentsAsync();
        Task<APiResponds<string>> DeleteAssignmentAsync(int id);
    }
}
