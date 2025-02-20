using System.Text.Json.Serialization;

namespace Minesweeper.Application.DTOs;

public record NewGameRequestDto(
    [property: JsonPropertyName("width")] int Width,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("mines_count")] int MinesCount
);
