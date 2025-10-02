using System.Collections.Generic;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Trainers_dto;
using FitFlex.Application.DTO_s.User_dto;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.CommenAPi;
using FitFlex.Domain.Enum;
namespace FitFlex.Application.Interfaces
{
    public interface IAuth
    {
        Task<APiResponds<LoginResponseDto>> Login(LoginDto dto);
        Task<APiResponds<string>> Register(RegisterDto dto);
        Task<APiResponds<UserResponseDto>> GetByUser(int id);
        Task<APiResponds<string>> TrainerRegistration(TrainerRegisterDto dto);
        Task<APiResponds<List<UserResponseDto>>> GetAllAsync();
        Task<APiResponds<TrainerResponseDto>> GetTrainerByID(int id);
        Task<APiResponds<string>> BlockUnBlock(int userId);
    }
}
