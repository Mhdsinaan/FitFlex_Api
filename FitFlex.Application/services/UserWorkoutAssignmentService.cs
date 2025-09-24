using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Entities.WorkoutPlan_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitFlex.Application.Services
{
    public class UserWorkoutAssignmentService : IUserWorkoutAssignmentService
    {
        private readonly IRepository<UserWorkoutAssignment> _assignmentRepo;
        private readonly IRepository<WorkoutPlan> _workoutPlanRepo;

        public UserWorkoutAssignmentService(
             IRepository<UserWorkoutAssignment> assignmentRepo,
             IRepository<WorkoutPlan> workoutPlanRepo)
        {
            _assignmentRepo = assignmentRepo;
            _workoutPlanRepo = workoutPlanRepo;
        }

        public async Task<APiResponds<string>> AssignWorkoutPlanAsync(int userId, int workoutPlanId)
        {
            var plan = await _workoutPlanRepo.GetByIdAsync(workoutPlanId);
            if (plan == null)
                return new APiResponds<string>("404", "Workout plan not found", null);

            var assignment = new UserWorkoutAssignment
            {
                UserId = userId,
                WorkoutPlanId = workoutPlanId,
                Status = AssignmentStatus.Assigned
            };

            await _assignmentRepo.AddAsync(assignment);
            await _assignmentRepo.SaveChangesAsync();

            return new APiResponds<string>("200", "Workout plan assigned successfully", null);
        }

        public async Task<APiResponds<string>> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus newStatus)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
            if (assignment == null)
                return new APiResponds<string>("404", "Assignment not found", null);

         
            assignment.Status = newStatus;

            _assignmentRepo.Update(assignment);
            await _assignmentRepo.SaveChangesAsync();

            return new APiResponds<string>("200", "Assignment status updated", null);
        }


        public async Task<APiResponds<UserWorkoutAssignmentResponse>> GetAssignmentByIdAsync(int id)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(id);

            

            var plan = await _workoutPlanRepo.GetByIdAsync(id);
            if (plan==null && assignment.IsDelete) return new APiResponds<UserWorkoutAssignmentResponse>("404", "plan not found", null);





            if (assignment == null)
                return new APiResponds<UserWorkoutAssignmentResponse>("404", "Assignment not found", null);

            var response = new UserWorkoutAssignmentResponse
            {
                Id = assignment.Id,
                UserId = assignment.UserId,
                PlanId = assignment.WorkoutPlanId,
                Name = plan.Name,
                Status = assignment.Status.ToString()

            };

            return new APiResponds<UserWorkoutAssignmentResponse>("200", "Success", response);
        }

        public async Task<APiResponds<IEnumerable<UserWorkoutAssignmentResponse>>> GetAssignmentsByUserAsync(int userId)
        {
            var assignments = await _assignmentRepo.GetAllQueryable()
                .Include(a => a.WorkoutPlan)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            var response = assignments.Select(a => new UserWorkoutAssignmentResponse
            {
                Id = a.Id,
                UserId = a.UserId,
                PlanId = a.WorkoutPlanId,
                Name= a.WorkoutPlan.Name,
                Status = a.Status.ToString()
            });

            return new APiResponds<IEnumerable<UserWorkoutAssignmentResponse>>("200", "Success", response);
        }

        public async Task<APiResponds<List<UserWorkoutAssignmentResponse>>> GetAllAssignmentsAsync()
        {
            var assignments = await _assignmentRepo.GetAllQueryable()
                .Include(a => a.WorkoutPlan)
                .ToListAsync();

            var response = assignments.Select(a => new UserWorkoutAssignmentResponse
            {
                Id = a.Id,
                UserId = a.UserId,
                PlanId = a.WorkoutPlanId,
                Name = a.WorkoutPlan?.Name,
                Status = a.Status.ToString()
            }).ToList();

            return new APiResponds<List<UserWorkoutAssignmentResponse>>("200", "Success", response);
        }

        public async Task<APiResponds<string>> DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(id);

            if (assignment == null || assignment.IsDelete)
                return new APiResponds<string>("404", "Assignment not found", null);

            assignment.IsDelete = true;

            _assignmentRepo.Update(assignment); 
            await _assignmentRepo.SaveChangesAsync();

            return new APiResponds<string>("200", "Assignment deleted successfully", null);
        }

    }
}
