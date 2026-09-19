using BoardGameTracker.Common.Enums;

namespace BoardGameTracker.Common.DTOs;

public class SuggestionDto
{
    public int Id { get; set; }
    public SuggestionStatus Status { get; set; }
    public string SuggestedName { get; set; } = string.Empty;
    public int? BggId { get; set; }
    public string? BggThumbnailUrl { get; set; }
    public string PhotoImagePath { get; set; } = string.Empty;
    public string? SourceNote { get; set; }
    public string? ReviewNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}
