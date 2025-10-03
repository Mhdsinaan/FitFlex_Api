using System.Security.Claims;

namespace FitFlex.Middleware
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserIdMiddleware> _logger;

        public UserIdMiddleware(RequestDelegate next, ILogger<UserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract UserId
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("User ID: {UserId}", userId);
                context.Items["UserId"] = int.Parse(userId);
            }
            else
            {
                _logger.LogWarning("User ID not found in claims.");
            }

            // Extract TrainerId
            var trainerIdClaim = context.User.FindFirstValue("TrainerId");
            if (!string.IsNullOrEmpty(trainerIdClaim))
            {
                _logger.LogInformation("Trainer ID: {TrainerId}", trainerIdClaim);
                context.Items["TrainerId"] = int.Parse(trainerIdClaim);
            }
            else
            {
                _logger.LogWarning("Trainer ID not found in claims.");
            }

            await _next(context);
        }
    }
}
