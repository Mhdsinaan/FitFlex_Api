using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Application.DTO_s.Session_DTO;

namespace FitFlex.API.Controllers
{
  
        [Route("api/[controller]")]
        [ApiController]
        public class SessionController : ControllerBase
        {
            private readonly ISessions _sessionService;

            public SessionController(ISessions sessionService)
            {
                _sessionService = sessionService;
            }

            // ---------------- Existing entity-based endpoints ----------------

            [HttpPost("create-dto")]
            public async Task<IActionResult> CreateWithDto([FromBody] SessionCreationDto sessionDto)
            {
                if (sessionDto == null)
                    return BadRequest("Invalid session data");

                var result = await _sessionService.CreateSessionAsync(sessionDto);
                if (result == null)
                    return StatusCode(500, "Failed to create session");

                return Ok(result);
            }


            [HttpGet("all")]
            public async Task<IActionResult> GetAll()
            {
                try
                {
                    var sessions = await _sessionService.GetAllWithDtoAsync();
                    return Ok(sessions);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error fetching sessions: {ex.Message}");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                try
                {
                    var session = await _sessionService.GetByIdWithDtoAsync(id);
                    if (session == null)
                        return NotFound("Session not found");

                    return Ok(session);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error fetching session: {ex.Message}");
                }
            }

            [HttpGet("trainer/{trainerId}")]
            public async Task<IActionResult> GetByTrainerId(int trainerId)
            {
                try
                {
                    var sessions = await _sessionService.GetByTrainerIdWithDtoAsync(trainerId);
                    return Ok(sessions);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error fetching trainer sessions: {ex.Message}");
                }
            }

            //[HttpDelete("{id}")]
            //public async Task<IActionResult> Delete(int id)
            //{
            //    try
            //    {
            //        var deleted = await _sessionService.Delete(id);
            //        if (!deleted)
            //            return NotFound("Session not found");

            //        return Ok("Session deleted successfully");
            //    }
            //    catch (Exception ex)
            //    {
            //        return StatusCode(500, $"Error deleting session: {ex.Message}");
            //    }
            //}

        }
    }

