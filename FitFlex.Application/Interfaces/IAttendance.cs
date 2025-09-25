using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Attendance;

namespace FitFlex.Application.Interfaces
{
    public interface IAttendance
    {
                                  
        Task<APiResponds<List<AttendanceDto>>> GetAttendanceByUserAsync(int userId);                              
        Task<APiResponds<List<AttendanceDto>> >GetAttendanceByTrainerAsync(int trainerId);

        Task<APiResponds<bool>> PunchInAsync(PunchAttendanceDto dto, int userid);
        Task<APiResponds<bool>> PunchOutAsync(PunchAttendanceDto dto, int userId);
        Task<APiResponds<bool>> UpdateAttendanceStatusAsync(int attendanceId, Attendance status);
    }
}
