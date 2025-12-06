using FitFlex.Application.DTO_s.Diet_plan_dtos;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DietPlanController : ControllerBase
{
    private readonly IDietPlanService _dietPlanService;

    public DietPlanController(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    // Get diet plans assigned by the currently logged-in trainer
    [HttpGet("TrainerPlans")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> GetDietPlansByTrainer()
    {
        int trainerId = Convert.ToInt32(HttpContext.Items["UserId"]);

        var result = await _dietPlanService.GetDietPlansByTrainerAsync(trainerId);

        if (result == null || result.Data == null || !result.Data.Any())
            return NotFound(result);

        return Ok(result);
    }

    // Assign a diet plan to a user by the logged-in trainer
    [HttpPost("assign")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> AssignDietPlan([FromBody] DietPlanRequestDto request)
    {
        int trainerId = Convert.ToInt32(HttpContext.Items["TrainerId"]);

        var result = await _dietPlanService.AssignDietPlanAsync(request, trainerId);

        if (result.StatusCode == "200")
            return Ok(result);

        return StatusCode(int.Parse(result.StatusCode), result);
    }

    // Get diet plan for the currently logged-in user
    [HttpGet("MyPlan")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetMyDietPlan()
    {
        int userId = Convert.ToInt32(HttpContext.Items["TrainerId"]);

        var result = await _dietPlanService.GetDietPlanByUserIdAsync(userId);

        if (result == null || result.Data == null)
            return NotFound(result);

        return Ok(result);
    }

    // Add a meal to a diet plan (Trainer only)
    [HttpPost("{dietPlanId}/meal")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> AddMealToDietPlan(int dietPlanId, [FromBody] MealRequestDto mealRequest)
    {
        var result = await _dietPlanService.AddMealToDietPlanAsync(dietPlanId, mealRequest);

        if (result.StatusCode == "200")
            return Ok(result);

        if (result.StatusCode == "404")
            return NotFound(result);

        return StatusCode(int.Parse(result.StatusCode), result);
    }

    // Remove a meal from a diet plan (Trainer only)
    [HttpDelete("{dietPlanId}/meal/{mealId}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> RemoveMealFromDietPlan(int dietPlanId, int mealId)
    {
        var result = await _dietPlanService.RemoveMealFromDietPlanAsync(dietPlanId, mealId);

        if (result.StatusCode == "200")
            return Ok(result);

        if (result.StatusCode == "404")
            return NotFound(result);

        return StatusCode(int.Parse(result.StatusCode), result);
    }

    // Update a diet plan (Trainer only)
    [HttpPut("{dietPlanId}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> UpdateDietPlan(int dietPlanId, [FromBody] DietPlanUpdateDto updateDto)
    {
        var result = await _dietPlanService.UpdateDietPlanAsync(dietPlanId, updateDto);

        if (result.StatusCode == "200")
            return Ok(result);

        if (result.StatusCode == "404")
            return NotFound(result);

        return StatusCode(int.Parse(result.StatusCode), result);
    }

    // Delete a diet plan (Trainer only)
    [HttpDelete("{dietPlanId}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> DeleteDietPlan(int dietPlanId)
    {
        var result = await _dietPlanService.DeleteDietPlanAsync(dietPlanId);

        if (result.StatusCode == "200")
            return Ok(result);

        if (result.StatusCode == "404")
            return NotFound(result);

        return StatusCode(int.Parse(result.StatusCode), result);
    }
}
