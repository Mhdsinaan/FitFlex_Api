using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Trainers_dto;
using FitFlex.Application.DTO_s.User_dto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace FitFlex.Application.services
{
    public class AuthService : IAuth
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Trainer> _trainerRepo;

        public AuthService(IRepository<User> userRepo, IRepository<Trainer> trainerRepo)
        {
            _userRepo = userRepo;
            _trainerRepo = trainerRepo;
        }

        
        public async Task<APiResponds<UserResponseDto>> GetByUser(int id)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null || user.IsDelete)
                    return new APiResponds<UserResponseDto>("404", "User not found", null);

                var dto = new UserResponseDto
                {
                    Id = user.ID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };

                return new APiResponds<UserResponseDto>("200", "User fetched successfully", dto);
            }
            catch (Exception ex)
            {
                return new APiResponds<UserResponseDto>("500", ex.Message, null);
            }
        }

       
        public async Task<APiResponds<List<UserResponseDto>>> GetAllAsync()
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                if (users == null || !users.Any())
                    return new APiResponds<List<UserResponseDto>>("404", "No users found", new List<UserResponseDto>());

                var userDtos = users
                    .Where(u => !u.IsDelete)
                    .Select(u => new UserResponseDto
                    {
                        Id = u.ID,
                        UserName = u.UserName,
                        Email = u.Email,
                        Role = u.Role.ToString()
                    })
                    .ToList();

                return new APiResponds<List<UserResponseDto>>("200", "Users fetched successfully", userDtos);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<UserResponseDto>>("500", ex.Message, null);
            }
        }

       
        public async Task<APiResponds<TrainerResponseDto>> GetTrainerByID(int id)
        {
            try
            {
                var trainer = await _trainerRepo.GetByIdAsync(id);
                if (trainer == null || trainer.IsDelete)
                    return new APiResponds<TrainerResponseDto>("404", "Trainer not found", null);

                var dto = new TrainerResponseDto
                {
                    Id = trainer.Id,
                    FullName = trainer.FullName,
                    PhoneNumber = trainer.PhoneNumber,
                    Gender = trainer.Gender,
                    ExperienceYears = trainer.ExperienceYears
                };

                return new APiResponds<TrainerResponseDto>("200", "Trainer fetched successfully", dto);
            }
            catch (Exception ex)
            {
                return new APiResponds<TrainerResponseDto>("500", ex.Message, null);
            }
        }

        
        public async Task<APiResponds<LoginResponseDto>> Login(LoginDto dto)
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                var loginData = users.FirstOrDefault(u =>
                    u.Email == dto.Email &&
                    u.Password == dto.Password &&
                    !u.IsDelete);

                if (loginData == null)
                    return new APiResponds<LoginResponseDto>("401", "Invalid email or password", null);

                var token = CreateToken(loginData);

                var response = new LoginResponseDto
                {
                    Email = loginData.Email,
                    UserName = loginData.UserName,
                    Role = loginData.Role.ToString(),
                    Token = token
                };

                return new APiResponds<LoginResponseDto>("200", "Login successful", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<LoginResponseDto>("500", ex.Message, null);
            }
        }

        
        public async Task<APiResponds<string>> Register(RegisterDto dto)
        {
            try
            {
                var exist = await _userRepo.GetAllAsync();
                if (exist.Any(u => u.Email == dto.Email))
                    return new APiResponds<string>("400", "User already exists", null);

                var user = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = UserRole.user
                };

                await _userRepo.AddAsync(user);
                await _userRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "Registration successful", user.Email);
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", ex.Message, null);
            }
        }

        
        public async Task<APiResponds<string>> TrainerRegistration(TrainerRegisterDto dto)
        {
            try
            {
                var existingUser = (await _userRepo.GetAllAsync())
                                    .FirstOrDefault(u => u.Email == dto.Email);

                if (existingUser != null)
                    return new APiResponds<string>("400", "Trainer already exists", null);

               
                var newUser = new User
                {
                    UserName = dto.FullName,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = UserRole.Trainer
                };

                await _userRepo.AddAsync(newUser);
                await _userRepo.SaveChangesAsync();

                
                var newTrainer = new Trainer
                {
                    FullName = dto.FullName,
                    ExperienceYears = dto.ExperienceYears,
                    Email = dto.Email,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    UserId = newUser.ID
                };

                await _trainerRepo.AddAsync(newTrainer);
                await _trainerRepo.SaveChangesAsync();

                return new APiResponds<string>("200", "New trainer created successfully", newTrainer.Email);
            }
            catch (Exception ex)
            {
                return new APiResponds<string>("500", ex.Message, null);
            }
        }

        // Token creation
        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("muhammedsinandotnetdeveloperatbridgeon");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Audience = "myusers",
                Issuer = "MyApp",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
