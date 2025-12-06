using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Trainers_dto;
using FitFlex.Application.DTO_s.User_dto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.Interfaces
{
    public interface ITrainerservice
    {
        Task<APiResponds<List<TrainerResponseDto>>> GetAllTrainersAsync();
        Task<APiResponds<TrainerResponseDto>> GetTrainerByIdAsync(int trainerId);
        Task<APiResponds<bool>> UpdateTrainerAsync(int trainerId, TrainerUpdateDto dto);
        Task<APiResponds<User?>> DeleteTrainerAsync(int trainerId, int currentUserId);
        Task<APiResponds<TrainerResponseDto?>> UpdateTrainerStatusAsync(int trainerId, TrainerStatus newStatus,Trainershift shift);



        Task<APiResponds<string>> ChangeTrainerAsync(int userId, int newTrainerId, int currentUserId);
        Task<APiResponds<List<UserTrainerResponseDto>>> GetAllAssignedUsers();
     



       
       
       
        

       
       




    }
}
