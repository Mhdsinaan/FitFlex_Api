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
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("User ID: {UserId}", userId);
                context.Items["UserId"] = userId;
            }
            else
            {
                _logger.LogWarning("User ID not found in claims.");
            }

            await _next(context);  
        }
    }
}
