using Ardalis.GuardClauses;
using BoardGameTracker.Common.Entities.Helpers;

namespace BoardGameTracker.Common.Entities;

public class ApiToken : HasId
{
    private string _name = string.Empty;
    private string _tokenHash = string.Empty;

    public string Name
    {
        get => _name;
        private set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public string TokenHash
    {
        get => _tokenHash;
        private set => _tokenHash = Guard.Against.NullOrWhiteSpace(value);
    }

    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUsedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt != null;

    public ApiToken(string name, string tokenHash)
    {
        Name = name;
        TokenHash = tokenHash;
        CreatedAt = DateTime.UtcNow;
    }

    public void RecordUsage()
    {
        LastUsedAt = DateTime.UtcNow;
    }

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
