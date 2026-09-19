using Ardalis.Specification;
using BoardGameTracker.Common.Entities;

namespace BoardGameTracker.Core.Auth.Specifications;

public sealed class ApiTokenByHashSpec : SingleResultSpecification<ApiToken>
{
    public ApiTokenByHashSpec(string tokenHash)
    {
        Query.Where(x => x.TokenHash == tokenHash);
    }
}
