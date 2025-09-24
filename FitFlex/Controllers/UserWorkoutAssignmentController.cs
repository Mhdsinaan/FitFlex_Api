using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserWorkoutAssignmentController : ControllerBase
{
    private readonly IUserWorkoutAssignmentService _assignmentService;

    public UserWorkoutAssignmentController(IUserWorkoutAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignWorkoutPlan(int userId, int workoutPlanId)
    {
        var result = await _assignmentService.AssignWorkoutPlanAsync(userId, workoutPlanId);
        if (result == null) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{assignmentId}/status")]
    public async Task<IActionResult> UpdateAssignmentStatus(int assignmentId, [FromQuery] AssignmentStatus status)
    {
        var result = await _assignmentService.UpdateAssignmentStatusAsync(assignmentId, status);
        if (result == null) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssignmentById(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment == null) return NotFound("Assignment not found");
        return Ok(assignment);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAssignmentsByUser(int userId)
    {
        var assignments = await _assignmentService.GetAssignmentsByUserAsync(userId);
        if (assignments == null) return NotFound(assignments);
        return Ok(assignments);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAssignments()
    {
        var assignments = await _assignmentService.GetAllAssignmentsAsync();
        if (assignments == null) return NotFound(assignments);
        return Ok(assignments);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var result = await _assignmentService.DeleteAssignmentAsync(id);
        if (result == null) return NotFound(result);
        return Ok(result);
    }
}
