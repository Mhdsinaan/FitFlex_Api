using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Session_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.Services
{
    public class UserSessionService : IUserSession
    {
        private readonly IRepository<UserSession> _userSessionRepo;
        private readonly IRepository<Session> _sessionRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Trainer> _TrainerRepo;

        public UserSessionService(
            IRepository<UserSession> userSessionRepo,
            IRepository<Session> sessionRepo,
            IRepository<User> userRepo,
             IRepository<Trainer> TrainerRepo)
        {
            _userSessionRepo = userSessionRepo;
            _sessionRepo = sessionRepo;
            _TrainerRepo = TrainerRepo;

        }


        public async Task<APiResponds<string>> AssignSessionAsync(int sessionId, int trainerId)
        {
           
            var trainer = await _TrainerRepo.GetByIdAsync(trainerId);
            if (trainer == null )
                return new APiResponds<string>("404", "Trainer not found", null);

           
            var session = await _sessionRepo.GetByIdAsync(sessionId);
            if (session == null)
                return new APiResponds<string>("404", "Session not found", null);

            
            var existingAssignments = await _userSessionRepo.GetAllAsync();
            var exists = existingAssignments.FirstOrDefault(x => x.TrainerID == trainerId && x.SessionId == sessionId);
            if (exists != null)
                return new APiResponds<string>("400", "Session already assigned to this trainer", null);

           
            var assignment = new UserSession
            {
                TrainerID = trainerId,
                SessionId = sessionId
            };

            await _userSessionRepo.AddAsync(assignment);
            await _userSessionRepo.SaveChangesAsync();

            return new APiResponds<string>("200", "Session assigned successfully", null);
        }



        public async Task<APiResponds<List<SessionResponseDto>>> GetAllAssignmentsAsync()
        {
            var sessions = await _sessionRepo.GetAllAsync();

            var response = sessions.Select(s => new SessionResponseDto
            {
                Id = s.Id,
                SessionName = s.Name,
                Details = s.Details,
                TimeSlot = s.TimeSlot.ToString()
            }).ToList();

            return new APiResponds<List<SessionResponseDto>>("200", "Sessions fetched successfully", response);
        }

       
       
    }
}
