using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Session_model;

namespace FitFlex.Application.Interfaces
{
    public interface IUserSession
    {
        Task<APiResponds<Session>> CreateSessionAsync(Session session);
        Task<APiResponds<Session>> GetByIdAsync(int sessionId);
        Task<APiResponds<IEnumerable<Session>>> GetAllAsync();
        Task<IEnumerable<Session>> GetByTrainerIdAsync(int trainerId);
        Task<bool> DeleteSessionAsync(int sessionId);
    }
}
