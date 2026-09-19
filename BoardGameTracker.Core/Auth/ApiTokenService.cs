using System.Security.Cryptography;
using BoardGameTracker.Common.Entities;
using BoardGameTracker.Core.Auth.Interfaces;
using BoardGameTracker.Core.Auth.Specifications;
using BoardGameTracker.Core.Datastore.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoardGameTracker.Core.Auth;

public class ApiTokenService : IApiTokenService
{
    private const string TokenPrefix = "bgt_";

    private readonly IRepository<ApiToken> _apiTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApiTokenService> _logger;

    public ApiTokenService(IRepository<ApiToken> apiTokenRepository, IUnitOfWork unitOfWork, ILogger<ApiTokenService> logger)
    {
        _apiTokenRepository = apiTokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ApiToken>> GetAll()
    {
        var tokens = await _apiTokenRepository.GetAllAsync();
        return tokens.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public async Task<(ApiToken Token, string RawToken)> Generate(string name)
    {
        var rawToken = TokenPrefix + Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");

        var token = new ApiToken(name, Hash(rawToken));
        await _apiTokenRepository.CreateAsync(token);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Api token {ApiTokenId} ({Name}) created", token.Id, token.Name);

        return (token, rawToken);
    }

    public async Task<ApiToken?> Validate(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return null;
        }

        var token = await _apiTokenRepository.SingleOrDefaultAsync(new ApiTokenByHashSpec(Hash(rawToken)));
        if (token == null || token.IsRevoked)
        {
            return null;
        }

        token.RecordUsage();
        await _unitOfWork.SaveChangesAsync();

        return token;
    }

    public async Task Revoke(int id)
    {
        var token = await _apiTokenRepository.GetByIdAsync(id);
        if (token == null)
        {
            return;
        }

        token.Revoke();
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Api token {ApiTokenId} revoked", id);
    }

    private static string Hash(string rawToken)
    {
        var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexString(bytes);
    }
}
