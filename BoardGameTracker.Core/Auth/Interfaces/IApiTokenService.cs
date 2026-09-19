using BoardGameTracker.Common.Entities;

namespace BoardGameTracker.Core.Auth.Interfaces;

public interface IApiTokenService
{
    Task<List<ApiToken>> GetAll();
    Task<(ApiToken Token, string RawToken)> Generate(string name);
    Task<ApiToken?> Validate(string rawToken);
    Task Revoke(int id);
}
