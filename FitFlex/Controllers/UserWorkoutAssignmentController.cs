using FitFlex.Application.DTO_s.workout_DTO;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    [Authorize(Roles = "Trainer")]

    public async Task<IActionResult> AssignWorkout([FromBody] AssignWorkoutRequest request)
    {
        int trainerId = Convert.ToInt32(HttpContext.Items["UserId"]);
        var result = await _assignmentService.AssignWorkoutAsync(request, trainerId);
        if (result == null) return BadRequest(result);
        return Ok(result);
    }


    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllWorkoutsByUser(int userId)
    {
        var result = await _assignmentService.GetAllWorkoutsByUserAsync(userId);
        if (result == null || result.Data == null) return NotFound(result);
        return Ok(result);
    }


    [HttpGet("user/{userId}/today")]
    public async Task<IActionResult> GetTodayWorkoutsByUser(int userId)
    {
        var result = await _assignmentService.GetTodayWorkoutsByUserAsync(userId);
        if (result == null || result.Data == null) return NotFound(result);
        return Ok(result);
    }


    [HttpPut("{assignmentId}/status")]
    public async Task<IActionResult> UpdateAssignmentStatus(int assignmentId, [FromQuery] AssignmentStatus status)
    {
        var result = await _assignmentService.UpdateAssignmentStatusAsync(assignmentId, status);
        if (result == null) return NotFound(result);
        return Ok(result);
    }

    // Get a single assignment by ID
    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetAssignmentById(int id)
    //{
    //    var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
    //    if (assignment == null) return NotFound("Assignment not found");
    //    return Ok(assignment);


    // Delete an assignment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var result = await _assignmentService.DeleteAssignmentAsync(id);
        if (result == null) return NotFound(result);
        return Ok(result);
    }
}

    
   

