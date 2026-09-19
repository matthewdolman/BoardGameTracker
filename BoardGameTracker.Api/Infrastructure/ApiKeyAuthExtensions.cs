using Microsoft.AspNetCore.Builder;

namespace BoardGameTracker.Api.Infrastructure;

public static class ApiKeyAuthExtensions
{
    public static IApplicationBuilder UseApiKeyAuthMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiKeyAuthMiddleware>();
    }
}
