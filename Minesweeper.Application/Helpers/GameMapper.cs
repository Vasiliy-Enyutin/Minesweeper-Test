using Minesweeper.Application.DTOs;
using Minesweeper.Domain.Entities;

namespace Minesweeper.Application.Helpers;

public static class GameMapper
{
    public static GameResponseDto MapToResponse(Game game)
    {
        var field = new List<List<string>>();
        for (var i = 0; i < game.Height; i++)
        {
            var row = new List<string>();
            for (var j = 0; j < game.Width; j++)
            {
                if (game.Status == GameStatus.Active)
                {
                    row.Add(game.Opened[i, j] 
                        ? (game.AdjacentMines[i, j] == 0 ? "0" : game.AdjacentMines[i, j].ToString()) 
                        : " ");
                }
                else
                {
                    row.Add(game.Mines[i, j] 
                        ? (game.Status == GameStatus.Lose ? "X" : "M") 
                        : game.AdjacentMines[i, j].ToString());
                }
            }
            field.Add(row);
        }

        return new GameResponseDto(
            game.Id,
            game.Width,
            game.Height,
            game.MinesCount,
            field,
            game.Status != GameStatus.Active
        );
    }
}