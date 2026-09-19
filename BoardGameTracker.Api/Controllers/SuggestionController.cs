using BoardGameTracker.Common;
using BoardGameTracker.Common.DTOs.Commands;
using BoardGameTracker.Common.Enums;
using BoardGameTracker.Common.Extensions;
using BoardGameTracker.Core.Suggestions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTracker.Api.Controllers;

[ApiController]
[Route("api/suggestion")]
[Authorize(Roles = Constants.AuthRoles.UserOrAdmin)]
public class SuggestionController : ControllerBase
{
    private readonly ISuggestionService _suggestionService;

    public SuggestionController(ISuggestionService suggestionService)
    {
        _suggestionService = suggestionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuggestions([FromQuery] SuggestionStatus? status)
    {
        var suggestions = await _suggestionService.GetList(status);
        return Ok(suggestions.ToListDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSuggestions([FromBody] CreateSuggestionsCommand command)
    {
        var suggestions = await _suggestionService.CreateBatch(command);
        return Ok(suggestions.ToListDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ReviseSuggestion(int id, [FromBody] ReviseSuggestionCommand command)
    {
        var suggestion = await _suggestionService.Revise(id, command);
        return Ok(suggestion.ToDto());
    }

    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> ApproveSuggestion(int id)
    {
        var suggestion = await _suggestionService.Approve(id);
        return Ok(suggestion.ToDto());
    }

    [HttpPut("{id:int}/reject")]
    public async Task<IActionResult> RejectSuggestion(int id, [FromBody] ReviewSuggestionCommand command)
    {
        var suggestion = await _suggestionService.Reject(id, command.Note);
        return Ok(suggestion.ToDto());
    }

    [HttpPut("{id:int}/retry")]
    public async Task<IActionResult> RequestRetry(int id, [FromBody] ReviewSuggestionCommand command)
    {
        var suggestion = await _suggestionService.RequestRetry(id, command.Note);
        return Ok(suggestion.ToDto());
    }
}
