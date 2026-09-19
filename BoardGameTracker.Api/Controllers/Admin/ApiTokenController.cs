using BoardGameTracker.Api.Infrastructure;
using BoardGameTracker.Common;
using BoardGameTracker.Common.DTOs.Auth;
using BoardGameTracker.Common.Extensions;
using BoardGameTracker.Core.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/api-tokens")]
[Authorize(Roles = Constants.AuthRoles.Admin)]
[ServiceFilter(typeof(AuthDisabledFilter))]
public class ApiTokenController : ControllerBase
{
    private readonly IApiTokenService _apiTokenService;

    public ApiTokenController(IApiTokenService apiTokenService)
    {
        _apiTokenService = apiTokenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTokens()
    {
        var tokens = await _apiTokenService.GetAll();
        return Ok(tokens.ToListDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] CreateApiTokenRequest request)
    {
        var (token, rawToken) = await _apiTokenService.Generate(request.Name);
        return Ok(new ApiTokenCreatedDto(token.Id, token.Name, rawToken, token.CreatedAt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> RevokeToken(int id)
    {
        await _apiTokenService.Revoke(id);
        return NoContent();
    }
}
