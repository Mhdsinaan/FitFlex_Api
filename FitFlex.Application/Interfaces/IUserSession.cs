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
    public interface IUserSession
    {
        Task<APiResponds<List<SessionResponseDto>>> GetAllAssignmentsAsync();
        Task<APiResponds<string>> AssignSessionAsync(int TrainerID, int sessionId);


    }
}
