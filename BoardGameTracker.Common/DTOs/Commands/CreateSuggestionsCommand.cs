namespace BoardGameTracker.Common.DTOs.Commands;

public class CreateSuggestionsCommand
{
    public required string PhotoImagePath { get; set; }
    public required List<CreateSuggestionItem> Suggestions { get; set; }
}

public class CreateSuggestionItem
{
    public required string SuggestedName { get; set; }
    public int? BggId { get; set; }
    public string? BggThumbnailUrl { get; set; }
    public string? SourceNote { get; set; }
}

public class ReviewSuggestionCommand
{
    public string? Note { get; set; }
}

public class ReviseSuggestionCommand
{
    public required string SuggestedName { get; set; }
    public int? BggId { get; set; }
    public string? BggThumbnailUrl { get; set; }
}
