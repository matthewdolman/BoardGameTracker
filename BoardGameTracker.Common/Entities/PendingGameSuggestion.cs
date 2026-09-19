using Ardalis.GuardClauses;
using BoardGameTracker.Common.Entities.Helpers;
using BoardGameTracker.Common.Enums;

namespace BoardGameTracker.Common.Entities;

public class PendingGameSuggestion : HasId
{
    private string _suggestedName = string.Empty;
    private string _photoImagePath = string.Empty;
    private SuggestionStatus _status;

    public string SuggestedName
    {
        get => _suggestedName;
        private set => _suggestedName = Guard.Against.NullOrWhiteSpace(value);
    }

    public SuggestionStatus Status
    {
        get => _status;
        private set => _status = Guard.Against.EnumOutOfRange(value);
    }

    public int? BggId { get; private set; }
    public string? BggThumbnailUrl { get; private set; }

    public string PhotoImagePath
    {
        get => _photoImagePath;
        private set => _photoImagePath = Guard.Against.NullOrWhiteSpace(value);
    }

    public string? SourceNote { get; private set; }
    public string? ReviewNote { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReviewedAt { get; private set; }

    public PendingGameSuggestion(string suggestedName, string photoImagePath, int? bggId = null,
        string? bggThumbnailUrl = null, string? sourceNote = null)
    {
        SuggestedName = suggestedName;
        PhotoImagePath = photoImagePath;
        BggId = bggId;
        BggThumbnailUrl = bggThumbnailUrl;
        SourceNote = sourceNote;
        Status = SuggestionStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revise(string suggestedName, int? bggId, string? bggThumbnailUrl)
    {
        SuggestedName = suggestedName;
        BggId = bggId;
        BggThumbnailUrl = bggThumbnailUrl;
        ReviewNote = null;
        Status = SuggestionStatus.Pending;
    }

    public void Approve()
    {
        Status = SuggestionStatus.Approved;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Reject(string? note)
    {
        Status = SuggestionStatus.Rejected;
        ReviewNote = note;
        ReviewedAt = DateTime.UtcNow;
    }

    public void RequestRetry(string? note)
    {
        Status = SuggestionStatus.RetryRequested;
        ReviewNote = note;
        ReviewedAt = DateTime.UtcNow;
    }
}
