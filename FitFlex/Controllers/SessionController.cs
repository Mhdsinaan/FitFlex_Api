using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Application.DTO_s.Session_DTO;
using FitFlex.Domain.Enum;
using FitFlex.CommenAPi;

namespace FitFlex.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessions _sessionService;
        private readonly IUserSession _userSessionService;

        public SessionController(ISessions sessionService, IUserSession userSessionService)
        {
            _sessionService = sessionService;
            _userSessionService = userSessionService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateWithDto( SessionCreationDto sessionDto,SessionTime time)
        {

            int UserID = Convert.ToInt32(HttpContext.Items["UserId"]);
            if (sessionDto == null)
                return BadRequest("Invalid session data");

            var result = await _sessionService.CreateSessionAsync(sessionDto, time,UserID);
            if (result == null)
                return StatusCode(500, "Failed to create session");

            return Ok(result);
        }


        //[HttpGet("all")]
        //public async Task<IActionResult> GetAll()
        //{
            
            
        //        var sessions = await _sessionService.GetAllWithDtoAsync();
        //    if (sessions == null)
        //        return NotFound(sessions);

        //    return Ok(sessions);
           
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            
                var session = await _sessionService.GetByIdWithDtoAsync(id);
                if (session == null)
                    return NotFound(session);

                return Ok(session);
         
            
            
        }

        //[HttpGet("trainer/{trainerId}")]
        //public async Task<IActionResult> GetByTrainerId(int trainerId)
        //{
        //    try
        //    {
        //        var sessions = await _sessionService.GetByTrainerIdWithDtoAsync(trainerId);
        //        return Ok(sessions);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error fetching trainer sessions: {ex.Message}");
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            int UserID = Convert.ToInt32(HttpContext.Items["UserId"]);
            var deleted = await _sessionService.DeleteSession(id,UserID);
            if (deleted is null)
                return NotFound();

            return Ok();



        }
        [HttpPost("assign")]
        public async Task<ActionResult<APiResponds<string>>> AssignSession([FromBody] AssignSessionRequestDto dto)
        {
            if (dto == null || dto.SessionId <= 0)
                return BadRequest(new APiResponds<string>("400", "Invalid request", null));

            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized(new APiResponds<string>("401", "Trainer not authenticated", null));

            int trainerId = Convert.ToInt32(HttpContext.Items["UserId"]);

            // Call service method
            var result = await _userSessionService.AssignSessionAsync(dto.SessionId, trainerId);

            if (result.StatusCode == "404")
                return NotFound(result);

            if (result.StatusCode == "400")
                return BadRequest(result);

            return Ok(result);
        }

        



        [HttpGet("all")]
        public async Task<ActionResult<APiResponds<List<SessionResponseDto>>>> GetAllAssignments()
        {
            var result = await _userSessionService.GetAllAssignmentsAsync();
            return Ok(result);
        }

    }
    }

