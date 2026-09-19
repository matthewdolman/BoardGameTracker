using BoardGameTracker.Common.DTOs.Auth;
using BoardGameTracker.Common.Entities;

namespace BoardGameTracker.Common.Extensions;

public static class ApiTokenDtoExtensions
{
    public static ApiTokenDto ToDto(this ApiToken token)
    {
        return new ApiTokenDto(
            token.Id,
            token.Name,
            token.CreatedAt,
            token.LastUsedAt,
            token.IsRevoked);
    }

    public static List<ApiTokenDto> ToListDto(this IEnumerable<ApiToken> tokens)
    {
        return tokens.Select(t => t.ToDto()).ToList();
    }
}
