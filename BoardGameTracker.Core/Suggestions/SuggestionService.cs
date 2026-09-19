using BoardGameTracker.Common.DTOs.Commands;
using BoardGameTracker.Common.Entities;
using BoardGameTracker.Common.Enums;
using BoardGameTracker.Common.Exceptions;
using BoardGameTracker.Common.Models.Bgg;
using BoardGameTracker.Core.Datastore.Interfaces;
using BoardGameTracker.Core.Games.Interfaces;
using BoardGameTracker.Core.Suggestions.Interfaces;
using BoardGameTracker.Core.Suggestions.Specifications;
using Microsoft.Extensions.Logging;

namespace BoardGameTracker.Core.Suggestions;

public class SuggestionService : ISuggestionService
{
    private readonly IRepository<PendingGameSuggestion> _suggestionRepository;
    private readonly IBggImportService _bggImportService;
    private readonly IGameService _gameService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SuggestionService> _logger;

    public SuggestionService(IRepository<PendingGameSuggestion> suggestionRepository, IBggImportService bggImportService,
        IGameService gameService, IUnitOfWork unitOfWork, ILogger<SuggestionService> logger)
    {
        _suggestionRepository = suggestionRepository;
        _bggImportService = bggImportService;
        _gameService = gameService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<PendingGameSuggestion>> GetList(SuggestionStatus? status)
    {
        _logger.LogDebug("Fetching suggestions with status {Status}", status);
        return _suggestionRepository.ListAsync(new SuggestionsByStatusSpec(status));
    }

    public async Task<List<PendingGameSuggestion>> CreateBatch(CreateSuggestionsCommand command)
    {
        _logger.LogDebug("Creating {Count} suggestions from photo {PhotoImagePath}", command.Suggestions.Count, command.PhotoImagePath);

        var suggestions = command.Suggestions
            .Select(item => new PendingGameSuggestion(item.SuggestedName, command.PhotoImagePath, item.BggId, item.BggThumbnailUrl, item.SourceNote))
            .ToList();

        await _suggestionRepository.CreateRangeAsync(suggestions);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("{Count} suggestions created from photo {PhotoImagePath}", suggestions.Count, command.PhotoImagePath);

        return suggestions;
    }

    public async Task<PendingGameSuggestion> Approve(int id)
    {
        var suggestion = await GetOrThrow(id);

        if (suggestion.BggId.HasValue)
        {
            // Reuses the same import/dedupe path the manual "search BGG by id" endpoint already
            // uses, so an already-owned game with this BggId won't be duplicated.
            await _bggImportService.ImportGameFromBgg(new BggSearch
            {
                BggId = suggestion.BggId.Value,
                State = GameState.Owned
            });
        }
        else
        {
            await _gameService.CreateGameFromCommand(new CreateGameCommand
            {
                Title = suggestion.SuggestedName,
                State = GameState.Owned
            });
        }

        suggestion.Approve();
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Suggestion {SuggestionId} ({Name}) approved", suggestion.Id, suggestion.SuggestedName);

        return suggestion;
    }

    public async Task<PendingGameSuggestion> Reject(int id, string? note)
    {
        var suggestion = await GetOrThrow(id);
        suggestion.Reject(note);
        await _unitOfWork.SaveChangesAsync();

        return suggestion;
    }

    public async Task<PendingGameSuggestion> RequestRetry(int id, string? note)
    {
        var suggestion = await GetOrThrow(id);
        suggestion.RequestRetry(note);
        await _unitOfWork.SaveChangesAsync();

        return suggestion;
    }

    public async Task<PendingGameSuggestion> Revise(int id, ReviseSuggestionCommand command)
    {
        var suggestion = await GetOrThrow(id);
        suggestion.Revise(command.SuggestedName, command.BggId, command.BggThumbnailUrl);
        await _unitOfWork.SaveChangesAsync();

        return suggestion;
    }

    private async Task<PendingGameSuggestion> GetOrThrow(int id)
    {
        var suggestion = await _suggestionRepository.GetByIdAsync(id);
        if (suggestion == null)
        {
            throw new EntityNotFoundException(nameof(PendingGameSuggestion), id);
        }

        return suggestion;
    }
}
