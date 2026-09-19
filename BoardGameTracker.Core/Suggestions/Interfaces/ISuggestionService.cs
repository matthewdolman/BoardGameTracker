using BoardGameTracker.Common.DTOs.Commands;
using BoardGameTracker.Common.Entities;
using BoardGameTracker.Common.Enums;

namespace BoardGameTracker.Core.Suggestions.Interfaces;

public interface ISuggestionService
{
    Task<List<PendingGameSuggestion>> GetList(SuggestionStatus? status);
    Task<List<PendingGameSuggestion>> CreateBatch(CreateSuggestionsCommand command);
    Task<PendingGameSuggestion> Approve(int id);
    Task<PendingGameSuggestion> Reject(int id, string? note);
    Task<PendingGameSuggestion> RequestRetry(int id, string? note);
    Task<PendingGameSuggestion> Revise(int id, ReviseSuggestionCommand command);
}
