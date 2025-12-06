using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.Session_DTO;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.Interfaces
{
    public interface ISessions
    {
        Task<APiResponds<SessionResponseDto>> CreateSessionAsync(SessionCreationDto sessionDto, SessionTime time,int UserID);
        Task<APiResponds<SessionResponseDto>> GetByIdWithDtoAsync(int sessionId);
        Task<APiResponds<IEnumerable<SessionResponseDto>>> GetAllWithDtoAsync();
        //Task<APiResponds<IEnumerable<SessionResponseDto>>> GetByTrainerIdWithDtoAsync(int trainerId);
        Task<APiResponds<SessionResponseDto>>DeleteSession(int sessionId,int UserId);

    }
}
