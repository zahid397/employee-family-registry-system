using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EmployeeRegistry.API.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RoleAuthorizationMiddleware> _logger;

        public RoleAuthorizationMiddleware(
            RequestDelegate next,
            ILogger<RoleAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read role from request header
            if (context.Request.Headers.TryGetValue("X-User-Role", out var roleValues))
            {
                var role = roleValues.ToString();

                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "Header");
                context.User = new ClaimsPrincipal(identity);

                _logger.LogInformation("User role set to {Role} from header", role);
            }
            else
            {
                // Default role = Viewer
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Viewer")
                };

                var identity = new ClaimsIdentity(claims, "Default");
                context.User = new ClaimsPrincipal(identity);

                _logger.LogInformation("No role header found. Default role = Viewer");
            }

            await _next(context);
        }
    }
}
