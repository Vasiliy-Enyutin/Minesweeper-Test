using System.Text.Json.Serialization;

namespace Minesweeper.Application.DTOs;

public record TurnRequestDto(
    [property: JsonPropertyName("game_id")] Guid GameId,
    [property: JsonPropertyName("row")] int Row,
    [property: JsonPropertyName("col")] int Col
);