using System.Security.Claims;
using BoardGameTracker.Common;
using BoardGameTracker.Core.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BoardGameTracker.Api.Infrastructure;

/// <summary>
/// Lets a request authenticate via an "X-Api-Key" header instead of a JWT bearer token —
/// for background/service clients (e.g. an automation agent) that shouldn't hold a user
/// password. Runs before UseAuthentication, exactly like AuthDisabledMiddleware, and only
/// acts when the request isn't already authenticated by something else.
/// </summary>
public class ApiKeyAuthMiddleware
{
    private const string HeaderName = "X-Api-Key";

    private readonly RequestDelegate _next;

    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiTokenService apiTokenService)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            if (context.Request.Headers.TryGetValue(HeaderName, out var headerValue))
            {
                var token = await apiTokenService.Validate(headerValue.ToString());
                if (token != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, $"api-token-{token.Id}"),
                        new Claim(ClaimTypes.Name, token.Name),
                        new Claim(ClaimTypes.Role, Constants.AuthRoles.User),
                        new Claim("display_name", token.Name)
                    };

                    var identity = new ClaimsIdentity(claims, "ApiKey");
                    context.User = new ClaimsPrincipal(identity);
                }
            }
        }

        await _next(context);
    }
}
