using System.Text.Json.Serialization;

namespace Minesweeper.Application.DTOs;

public record GameResponseDto(
    [property: JsonPropertyName("game_id")] Guid GameId,
    [property: JsonPropertyName("width")] int Width,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("mines_count")] int MinesCount,
    [property: JsonPropertyName("field")] List<List<string>> Field,
    [property: JsonPropertyName("completed")] bool Completed
);