using Ardalis.Specification;
using BoardGameTracker.Common.Entities;
using BoardGameTracker.Common.Enums;

namespace BoardGameTracker.Core.Suggestions.Specifications;

public sealed class SuggestionsByStatusSpec : Specification<PendingGameSuggestion>
{
    public SuggestionsByStatusSpec(SuggestionStatus? status)
    {
        if (status.HasValue)
        {
            Query.Where(x => x.Status == status.Value);
        }

        Query
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking();
    }
}
