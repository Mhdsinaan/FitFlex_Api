using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFlex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlanController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            var result = await _workoutPlanService.GetAllWorkoutPlansAsync();
            if (result == null ) return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(int id)
        {
            var result = await _workoutPlanService.GetWorkoutPlanByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Trainer")]

        public async Task<IActionResult> CreateWorkoutPlan([FromBody] CreateWorkoutPlanRequest dto)
        {
            var result = await _workoutPlanService.CreateWorkoutPlanAsync(dto);
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, [FromBody] CreateWorkoutPlanRequest dto)
        {
            var result = await _workoutPlanService.UpdateWorkoutPlanAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            var result = await _workoutPlanService.DeleteWorkoutPlanAsync(id);
            if (result==null) return NotFound();
            return Ok(result);
        }
    }
}
