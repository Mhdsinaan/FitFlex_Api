using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
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
        private readonly IRepository<User> _Userrepo;
        private readonly IRepository<UserTrainer> _UserTrainer;
        public UserWorkoutAssignmentService(
             IRepository<UserWorkoutAssignment> assignmentRepo,
             IRepository<WorkoutPlan> workoutPlanRepo,
             IRepository<User> Userrepo,
             IRepository<UserTrainer> UserTrainer)
        {
            _assignmentRepo = assignmentRepo;
            _workoutPlanRepo = workoutPlanRepo;
            _Userrepo = Userrepo;
            _UserTrainer = UserTrainer;
        }

        public async Task<APiResponds<AssignWorkoutResponseDto>> AssignWorkoutAsync(AssignWorkoutRequest request, int TrainerId)
        {
            try
            {

                var assignedTrainer = await _UserTrainer.GetAllAsync(); // or better: query DB directly
                var match = assignedTrainer.FirstOrDefault(p => p.TrainerId == TrainerId && p.UserId == request.UserId);

                if (match == null)
                    return new APiResponds<AssignWorkoutResponseDto>("404", "This trainer is not assigned to this user", null);



                var workout = await _workoutPlanRepo.GetByIdAsync(request.WorkoutID);
                if (workout is null) return new APiResponds<AssignWorkoutResponseDto>("404", "workout plan notfound", null);

                var existingAssignment = await _assignmentRepo .GetAllAsync(); 
                                       

                var duplicate = existingAssignment.FirstOrDefault(a => a.UserId == request.UserId && a.WorkoutPlanId == request.WorkoutID);


                if (duplicate != null && duplicate.AssignmentStatus!=AssignmentStatus.Completed )
                {
                    return new APiResponds<AssignWorkoutResponseDto>("409", "Workout already assigned to this user", null);
                }

                
                var today = DateTime.UtcNow.Date;

                var assignment = new UserWorkoutAssignment()
                {
                   
                    UserId = request.UserId,
                    AssignmentStatus = AssignmentStatus.Assigned,
                    WorkoutPlanId = request.WorkoutID,
                    TrainerId=TrainerId,

                    CreatedBy = TrainerId,
                    CreatedOn = today,


                };

                await _assignmentRepo.AddAsync(assignment);
                await _assignmentRepo.SaveChangesAsync();

                var response = new AssignWorkoutResponseDto()
                {
                    UserId = assignment.UserId,
                    TrainerId = TrainerId,
                    AssignmentStatus=assignment.AssignmentStatus.ToString(),
                  
                    WorkoutId = assignment.WorkoutPlanId
                };

                return new APiResponds<AssignWorkoutResponseDto>("200", "Success", response);




            }catch (Exception ex)
            {
                return new APiResponds<AssignWorkoutResponseDto>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<string>> DeleteAssignmentAsync(int assignmentId)
        {
            try
            {
                // Fetch the assignment from the repository
                var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
                if (assignment == null)
                    return new APiResponds<string>("404", "Assignment not found", null);

                // Delete the assignment
                _assignmentRepo.Delete(assignment);
                await _assignmentRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "Assignment deleted successfully", "Deleted");
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<APiResponds<List<WorkoutDto>>> GetAllWorkoutsByUserAsync(int userId)
        {
            try
            {
                var assignments = await _assignmentRepo.GetAllAsync();

                var userWorkouts = assignments
                    .Where(a => a.UserId == userId)
                    .Select(a => new WorkoutDto
                    {
                        AssignmentId = a.Id,
                        UserId = a.UserId,
                        TrainerId = a.CreatedBy,
                        StartDate = a.CreatedOn,
                        //EndDate = a.EndDate,
                        Status = a.AssignmentStatus.ToString()
                    })
                    .ToList();

                if (userWorkouts == null || !userWorkouts.Any())
                    return new APiResponds<List<WorkoutDto>>("404", "No workouts found for this user", null);

                return new APiResponds<List<WorkoutDto>>("200", "Success", userWorkouts);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<WorkoutDto>>("500", ex.Message, null);
            }
        }


        public async Task<APiResponds<List<UserWorkoutAssignmentResponse>>> GetTodayWorkoutsByUserAsync(int userId)
        {
            try
            {
                var allWorkouts = await _assignmentRepo.GetAllAsync();

                var today = DateTime.UtcNow.Date; 
                var userTodayWorkouts = allWorkouts
                    .Where(w => w.UserId == userId && w.CreatedOn.Date == today)
                    .ToList();

                if (!userTodayWorkouts.Any())
                    return new APiResponds<List<UserWorkoutAssignmentResponse>>("404", "No workouts found for today", null);

                var workoutDtos = userTodayWorkouts.Select(w => new UserWorkoutAssignmentResponse
                {
                    Id = w.Id,
                    Name=w.WorkoutPlan.Name,
                    StartDate=today,
                   
                    AssignmentStatus=w.AssignmentStatus.ToString(),
                    WorkoutID=w.WorkoutPlan.Id,
                 
                }).ToList();

                return new APiResponds<List<UserWorkoutAssignmentResponse>>("200", "Today's workouts fetched successfully", workoutDtos);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<UserWorkoutAssignmentResponse>>("500", $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<APiResponds<WorkoutDto>> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus status)
        {
            try
            {
                // Fetch the assignment from the repository
                var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
                if (assignment == null)
                    return new APiResponds<WorkoutDto>("404", "Assignment not found", null);

                // Update the status
                assignment.AssignmentStatus = status;

                // Save changes
                await _assignmentRepo.SaveChangesAsync();

                // Map to DTO
                var workoutDto = new WorkoutDto
                {
                    AssignmentId = assignment.Id,
                    UserId = assignment.UserId,
                    TrainerId = assignment.CreatedBy,
                    StartDate = assignment.CreatedOn,
                    Status = assignment.AssignmentStatus.ToString()
                };

                return new APiResponds<WorkoutDto>("200", "Assignment status updated successfully", workoutDto);
            }
            catch (Exception ex)
            {
                return new APiResponds<WorkoutDto>("500", $"An error occurred: {ex.Message}", null);
            }
        }

    }
}
