using Microsoft.AspNetCore.Mvc;
using FitFlex.Application.services;
using FitFlex.Application.DTO_s;
using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Domain.Enum;
using System.Threading.Tasks;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Entities.Attendance;

namespace FitFlex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendance _attendanceService;

        public AttendanceController(IAttendance attendanceService)
        {
            _attendanceService = attendanceService;
        }


        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetByTrainer(int trainerId)
        {
            var result = await _attendanceService.GetAttendanceByTrainerAsync(trainerId);
            if (result.Data == null || result.Data.Count == 0)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser()
        {
            int userid = Convert.ToInt32(HttpContext.Items["UserId"]);

            var result = await _attendanceService.GetAttendanceByUserAsync(userid);
            if (result == null )
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("punchin")]
        public async Task<IActionResult> PunchIn([FromBody] PunchAttendanceDto dto)
        {
            int UserId = Convert.ToInt32(HttpContext.Items["UserId"]);
            if (UserId is 0) return Unauthorized(UserId);

            var result = await _attendanceService.PunchInAsync(dto, UserId);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

       
        [HttpPost("punchout")]
        public async Task<IActionResult> PunchOut([FromBody] PunchAttendanceDto dto)
        {
            int UserId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var result = await _attendanceService.PunchOutAsync(dto, UserId);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPut("updatestatus/{attendanceId}")]
        public async Task<IActionResult> UpdateStatus(int attendanceId, [FromBody] Attendance status)
        {

            var result = await _attendanceService.UpdateAttendanceStatusAsync(attendanceId, status);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

    }
}
