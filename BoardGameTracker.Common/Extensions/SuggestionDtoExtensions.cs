using BoardGameTracker.Common.DTOs;
using BoardGameTracker.Common.Entities;

namespace BoardGameTracker.Common.Extensions;

public static class SuggestionDtoExtensions
{
    public static SuggestionDto? ToDto(this PendingGameSuggestion? suggestion)
    {
        if (suggestion == null)
        {
            return null;
        }

        return new SuggestionDto
        {
            Id = suggestion.Id,
            Status = suggestion.Status,
            SuggestedName = suggestion.SuggestedName,
            BggId = suggestion.BggId,
            BggThumbnailUrl = suggestion.BggThumbnailUrl,
            PhotoImagePath = suggestion.PhotoImagePath,
            SourceNote = suggestion.SourceNote,
            ReviewNote = suggestion.ReviewNote,
            CreatedAt = suggestion.CreatedAt,
            ReviewedAt = suggestion.ReviewedAt
        };
    }

    public static List<SuggestionDto> ToListDto(this IEnumerable<PendingGameSuggestion> suggestions)
    {
        return suggestions
            .Select(s => s.ToDto())
            .OfType<SuggestionDto>()
            .ToList();
    }
}
