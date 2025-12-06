using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FitFlex.Application.DTO_s.Session_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 

namespace FitFlex.Application.Services
{
    public class SessionService : ISessions
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<Trainer> _Trainerrepo;

        public SessionService(IRepository<Session> sessionRepo, IMapper mapper, IRepository<Trainer> Trainerrepo)
        {
            _sessionRepo = sessionRepo;
            _mapper = mapper;
            _Trainerrepo = Trainerrepo;
        }

        public async Task<APiResponds<SessionResponseDto>> CreateSessionAsync(SessionCreationDto sessionDto, SessionTime time,int userID)
        {
            try
            {

                var existingSession = (await _sessionRepo.GetAllAsync());


                if (existingSession.Any(s => s.Name == sessionDto.Name)) 
                {
                    return new APiResponds<SessionResponseDto>("400", "This session already exists", null);
                }
                var newSession = new Session
                {
                    
                    Name = sessionDto.Name,
                    Details=sessionDto.Details,
                    TimeSlot=time,
                   


                    CreatedBy =userID,
                    CreatedOn=DateTime.UtcNow,
                };

                

                await _sessionRepo.AddAsync(newSession);
                await _sessionRepo.SaveChangesAsync();

                var responseDto = new SessionResponseDto
                {
                    Id = newSession.Id,
                    SessionName = newSession.Name,
                    TimeSlot = newSession.TimeSlot.ToString(),
                    Details=newSession.Details
                   

                };

                return new APiResponds<SessionResponseDto>("200", "Session created successfully", responseDto);
            }
            catch (Exception ex)
            {
                return new APiResponds<SessionResponseDto>("500", $"Error creating session: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<SessionResponseDto>> GetByIdWithDtoAsync(int sessionId)
        {
            try
            {
                var session = await _sessionRepo.GetByIdAsync(sessionId);
                if (session == null)
                    return new APiResponds<SessionResponseDto>("404", "Session not found", null);

                var responseDto = new SessionResponseDto
                {
                    Id = session.Id,
                    SessionName = session.Name,
                    Details = session.Details,
                    TimeSlot = session.TimeSlot.ToString(),
                   
                    
                };


                return new APiResponds<SessionResponseDto>("200", "Success", responseDto);
            }
            catch (Exception ex)
            {
                return new APiResponds<SessionResponseDto>("500", $"Error fetching session: {ex.Message}", null);
            }
        }

        public async Task<APiResponds<IEnumerable<SessionResponseDto>>> GetAllWithDtoAsync()
        {
            try
            {
                var sessions = await _sessionRepo.GetAllAsync();

                var responseDtos = sessions.Select(session => new SessionResponseDto
                {
                    Id = session.Id,
                    SessionName = session.Name,
                    Details = session.Details,
                    TimeSlot = session.TimeSlot.ToString(),
                
                }).ToList();

                return new APiResponds<IEnumerable<SessionResponseDto>>("200", "Success", responseDtos);
            }
            catch (Exception ex)
            {
                return new APiResponds<IEnumerable<SessionResponseDto>>("500", $"Error fetching sessions: {ex.Message}", null);
            }
        }

        //public async Task<APiResponds<IEnumerable<SessionResponseDto>>> GetByTrainerIdWithDtoAsync(int trainerId)
        //{
        //    try
        //    {
        //        var sessions = await _sessionRepo.GetAllAsync();
        //        var filtered = sessions.Where(s => s.TrainerId == trainerId);
        //        var responseDtos = _mapper.Map<IEnumerable<SessionResponseDto>>(filtered);

        //        return new APiResponds<IEnumerable<SessionResponseDto>>("200", "Success", responseDtos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APiResponds<IEnumerable<SessionResponseDto>>("500", $"Error fetching trainer sessions: {ex.Message}", null);
        //    }
        //}

        public async Task<APiResponds<SessionResponseDto>> DeleteSession(int sessionId,int UserID)
        {
            try
            {
                var session = await _sessionRepo.GetByIdAsync(sessionId);
                if (session is null) return new APiResponds<SessionResponseDto>("404", "session not found", null);
                session.IsDelete = true;
                session.DeletedBy = UserID;
                session.DeletedOn = DateTime.UtcNow;

                _sessionRepo.Update(session);
                await _sessionRepo.SaveChangesAsync();

                var responseDto = new SessionResponseDto
                {
                    Id = session.Id,
                    SessionName = session.Name,
                    TimeSlot = session.TimeSlot.ToString(),
                    Details=session.Details,
                    
                  

                };
                return new APiResponds<SessionResponseDto>("200", "session deleted sucess fully", responseDto);

            }
            catch (Exception ex)
            {
                return new APiResponds<SessionResponseDto>("500", $"error delete session : {ex.Message}", null);
            }

        }
    }
}
