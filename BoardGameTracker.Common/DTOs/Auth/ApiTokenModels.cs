namespace BoardGameTracker.Common.DTOs.Auth;

public record CreateApiTokenRequest(string Name);

public record ApiTokenDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime? LastUsedAt,
    bool IsRevoked);

public record ApiTokenCreatedDto(
    int Id,
    string Name,
    string Token,
    DateTime CreatedAt);
