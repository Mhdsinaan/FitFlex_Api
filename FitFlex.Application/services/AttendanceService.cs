using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Attendance;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.services
{
    public class AttendanceService : IAttendance
    {
        private readonly IRepository<Attendance> _AttendenceRepo;
        public AttendanceService(IRepository<Attendance> attendance)
        {
            _AttendenceRepo = attendance;
        }

        public async Task<APiResponds<List<AttendanceDto>>> GetAttendanceByTrainerAsync(int trainerId)
        {
            try
            {
                var byuser = await _AttendenceRepo.GetAllAsync();
                var alldata = byuser.Where(p => p.TrainerId == trainerId).ToList();

                if (alldata is null) return new APiResponds<List<AttendanceDto>>("404", "notfound", null);
                var all = byuser.Select(p => new AttendanceDto
                {
                    TrainerId = p.TrainerId,
                    SlotTime = p.Slot,
                    PunchIn = p.PunchIn,
                    PunchOut = p.PunchOut,
                    Status = p.Status,



                }).ToList();

                return new APiResponds<List<AttendanceDto>>("200", "Attendance details", all);

            }
            catch (Exception ex)
            {
                return new APiResponds<List<AttendanceDto>>("500", $"An error occurred: {ex.Message}", null);

            }
        }

        public async Task<APiResponds<List<AttendanceDto>>> GetAttendanceByUserAsync(int userId)
        {
            try
            {
                var allAttendances = await _AttendenceRepo.GetAllAsync();

                var userAttendances = allAttendances
                    .Where(p => p.UserId == userId)
                    .ToList(); 

                if (!userAttendances.Any())
                    return new APiResponds<List<AttendanceDto>>("404", "Data not found", null);

                var dtoList = userAttendances.Select(p => new AttendanceDto
                {
                    
                    TrainerId = p.TrainerId,
                    PunchIn = p.PunchIn,
                    PunchOut = p.PunchOut,
                    SlotTime =p.Slot,
                }).ToList();

                return new APiResponds<List<AttendanceDto>>("200", "Attendance details", dtoList);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<AttendanceDto>>("500", $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<APiResponds<bool>> PunchInAsync(PunchAttendanceDto dto,int userid)
        {
            try
            {
                var attendance = new Attendance
                {
                    UserId = userid,
                    TrainerId = dto.TrainerId,
                    Slot = dto.Slot, 
                    PunchIn = DateTime.Now,
                    Status = AttendanceStatus.Present
                };

                await _AttendenceRepo.AddAsync(attendance);

                return new APiResponds<bool>("200", "Punch-in successful", true);
            }
            catch (Exception ex)
            {
                return new APiResponds<bool>("500", $"Error: {ex.Message}", false);
            }
        }


        public async Task<APiResponds<bool>> PunchOutAsync(PunchAttendanceDto dto,int userId)
        {
            try
            {
              
                var attendance = (await _AttendenceRepo.GetAllAsync())
                    .FirstOrDefault(a => a.UserId == userId && a.Slot == dto.Slot && a.PunchOut == null);

                if (attendance == null)
                    return new APiResponds<bool>("404", "Punch-in record not found", false);

                attendance.PunchOut = DateTime.Now;

              _AttendenceRepo.Update(attendance);

                return new APiResponds<bool>("200", "Punch-out successful", true);
            }
            catch (Exception ex)
            {
                return new APiResponds<bool>("500", $"Error: {ex.Message}", false);
            }
        }


        public async Task<APiResponds<bool>> UpdateAttendanceStatusAsync(int attendanceId, Attendance status)
        {
            try
            {
                var attendance = await _AttendenceRepo.GetByIdAsync(attendanceId);
                if (attendance == null)
                    return new APiResponds<bool>("404", "Attendance not found", false);

                attendance.Status = status.Status;




                return new APiResponds<bool>("200", "Attendance status updated", true);
            }
            catch (Exception ex)
            {
                return new APiResponds<bool>("500", $"Error: {ex.Message}", false);
            }
        }

    }
}
