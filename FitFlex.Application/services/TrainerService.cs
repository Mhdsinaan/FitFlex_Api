using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Trainers_dto;
using FitFlex.Application.DTO_s.User_dto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.services
{
    public class TrainerService : ITrainerservice
    {
        private readonly IRepository<Trainer> _trainerRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<UserTrainer> _userTrainer;

        public TrainerService(IRepository<Trainer> trainerRepo, IRepository<User> userRepo, IRepository<UserTrainer> userTrainer)
        {
            _trainerRepo = trainerRepo;
            _userRepo = userRepo;
            _userTrainer = userTrainer;
        }

            public async Task<APiResponds<TrainerResponseDto?>> UpdateTrainerStatusAsync(int trainerId, TrainerStatus newStatus, Trainershift shift)
            {
                try
                {
                    var trainer = await _trainerRepo.GetByIdAsync(trainerId);
                    if (trainer == null || trainer.IsDelete)
                        return new APiResponds<TrainerResponseDto?>("404", "Trainer not found", null);

            

               
                    trainer.status = newStatus;
                     trainer.Shift = shift;
                    

                    trainer.ModifiedOn = DateTime.UtcNow;

                    _trainerRepo.Update(trainer);
                    await _trainerRepo.SaveChangesAsync();

                var dto = new TrainerResponseDto
                {
                    Id = trainer.Id,
                    FullName = trainer.FullName,
                    Email = trainer.Email,
                    PhoneNumber = trainer.PhoneNumber,
                    Gender = trainer.Gender,
                    ExperienceYears = trainer.ExperienceYears,
                    Status = trainer.status.ToString(),
                    CreatedOn = trainer.CreatedOn,
                    Shift = trainer.Shift.ToString()
                };

                    return new APiResponds<TrainerResponseDto?>("200", $"Trainer {newStatus} successfully", dto);
                }
                catch (Exception ex)
                {
                    return new APiResponds<TrainerResponseDto?>("500", ex.Message, null);
                }
            }

        public async Task<APiResponds<User?>> DeleteTrainerAsync(int trainerId, int currentUserId)
        {
            try
            {

                var trainer = await _trainerRepo.GetByIdAsync(trainerId);
                
                if (trainer == null || trainer.UserId == 0 || trainer.IsDelete==true)
                    return new APiResponds<User?>("404", "Trainer not found or already deleted", null);

                var user = await _userRepo.GetByIdAsync(trainerId);

                if (user == null || user.IsDelete)
                    return new APiResponds<User?>("404", "Trainer not found or already deleted", null);

                var assignment = await _userTrainer.GetAllAsync();
                var usertrainer = assignment.FirstOrDefault(p => p.TrainerId == trainerId && p.IsDelete !=true);

                if (usertrainer != null) return new APiResponds<User?>("400", "Trainer is assigned to a user and cannot be deleted", null);



                var userT = await _userRepo.GetByIdAsync(trainer.UserId);

                trainer.DeletedBy = currentUserId;
                trainer.DeletedOn = DateTime.UtcNow;
                trainer.IsDelete = true;
                user.IsDelete = true;

                _trainerRepo.Update(trainer);
                _userRepo.Update(userT);


                await _trainerRepo.SaveChangesAsync();
                await _userRepo.SaveChangesAsync();

                return new APiResponds<User?>("200", "Trainer deleted successfully", userT);
            }
            catch (Exception ex)
            {
                return new APiResponds<User?>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<List<TrainerResponseDto>>> GetAllTrainersAsync()
        {
            try
            {
                var trainers = (await _trainerRepo.GetAllAsync()).Where(t => !t.IsDelete).ToList();

                var dtoList = trainers.Select(t => new TrainerResponseDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    Gender = t.Gender,
                    ExperienceYears = t.ExperienceYears,
                    CreatedOn = t.CreatedOn,
                    Shift=t.Shift.ToString(),
                    Status=t.status.ToString()
                }).ToList();

                return new APiResponds<List<TrainerResponseDto>>("200", "Trainers fetched successfully", dtoList);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<TrainerResponseDto>>("500", ex.Message, null);
            }
        }

        public async Task<APiResponds<TrainerResponseDto>> GetTrainerByIdAsync(int trainerId)
        {
            try
            {
                var trainer = await _trainerRepo.GetByIdAsync(trainerId);
                if (trainer == null || trainer.status != TrainerStatus.Accept || trainer.IsDelete)
                    return new APiResponds<TrainerResponseDto>("404", "Trainer not found", null);

                var dto = new TrainerResponseDto
                {
                    Id = trainer.Id,
                    Email = trainer.Email,
                    FullName = trainer.FullName,
                    Gender = trainer.Gender,
                    PhoneNumber = trainer.PhoneNumber,
                    ExperienceYears = trainer.ExperienceYears,
                    Status = trainer.status.ToString(),
                    Shift = trainer.Shift.ToString()
                };

                return new APiResponds<TrainerResponseDto>("200", "Trainer fetched successfully", dto);
            }
            catch (Exception ex)
            {
                
                return new APiResponds<TrainerResponseDto>("500", "Internal server error", null);
            }
        }


        public async Task<APiResponds<bool>> UpdateTrainerAsync(int trainerId, TrainerUpdateDto dto)
        {
            try
            {
                var trainer = await _trainerRepo.GetByIdAsync(trainerId);
                if (trainer == null)
                    return new APiResponds<bool>("404", "Trainer not found", false);

                trainer.FullName = dto.FullName;
                trainer.PhoneNumber = dto.PhoneNumber;
                trainer.Gender = dto.Gender;
                trainer.ExperienceYears = dto.ExperienceYears;

                _trainerRepo.Update(trainer);
                await _trainerRepo.SaveChangesAsync();

                return new APiResponds<bool>("200", "Trainer updated successfully", true);
            }
            catch (Exception ex)
            {
                return new APiResponds<bool>("500", ex.Message, false);
            }
        }



        public async Task<APiResponds<List<TrainerResponseDto>>> TotalActiveTrainers()
        {
            var trainers = await _trainerRepo.GetAllAsync();

            
            var activeTrainers = trainers
                .Where(p => !p.IsDelete && p.status == TrainerStatus.Accept)
                .Select(p => new TrainerResponseDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Email = p.Email,
                    PhoneNumber= p.PhoneNumber,
                    ExperienceYears= p.ExperienceYears,
                    Status = TrainerStatus.Accept.ToString()
                })
                .ToList();

            if (!activeTrainers.Any())
                return new APiResponds<List<TrainerResponseDto>>("400", "No active trainers found", null);

            return new APiResponds<List<TrainerResponseDto>>("200", "Active trainers retrieved successfully", activeTrainers);
        }





        public async Task<APiResponds<List<UserTrainerResponseDto>>> GetAllAssignedUsers()
        {
            
            var assignedUsers = await _userTrainer.GetAllAsync();

           
            if (assignedUsers == null || !assignedUsers.Any())
                return new APiResponds<List<UserTrainerResponseDto>>("404", "No assigned users found", null);

            
            var assignedDtos = assignedUsers.Select(ut => new UserTrainerResponseDto
            {
                TrainerId = ut.Id,
                UserId = ut.UserId,
                UserName=ut.User.UserName,
                TrainerName = ut.Trainer.FullName, 
                AssignedDate = ut.CreatedOn
            }).ToList();

           
            return new APiResponds<List<UserTrainerResponseDto>>("200", "Assigned users retrieved successfully", assignedDtos);
        }

        public async Task<APiResponds<string>> ChangeTrainerAsync(int userId, int newTrainerId, int currentUserId)
        {
            try
            {
                var newTrainer = await _trainerRepo.GetByIdAsync(newTrainerId);
                if (newTrainer == null || newTrainer.IsDelete)
                    return new APiResponds<string>("404", "New trainer not found", null);

                var assignments = await _userTrainer.GetAllAsync();
                var existingAssignment = assignments.FirstOrDefault(p => p.UserId == userId && !p.IsDelete);

                if (existingAssignment == null)
                    return new APiResponds<string>("404", "User has no trainer assigned", null);

                
                if (existingAssignment.TrainerId == newTrainerId)
                    return new APiResponds<string>("403", "Trainer already assigned to this user", null);

                existingAssignment.TrainerId = newTrainerId;
                existingAssignment.ModifiedBy = currentUserId;
                existingAssignment.ModifiedOn = DateTime.UtcNow;

                _userTrainer.Update(existingAssignment);
                await _userTrainer.SaveChangesAsync();

                return new APiResponds<string>("200", "Trainer changed successfully", $"New TrainerId: {newTrainerId}");
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", ex.Message, null);
            }
        }


    }
}
