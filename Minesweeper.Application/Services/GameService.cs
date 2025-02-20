using Minesweeper.Application.DTOs;
using Minesweeper.Application.Helpers;
using Minesweeper.Domain.Entities;
using Minesweeper.Domain.Exceptions;
using Minesweeper.Domain.Interfaces;

namespace Minesweeper.Application.Services;

public class GameService(IGameRepository repository) : IGameService
{
    private const string GameCompletedMessage = "Game already completed";
    private const string InvalidCellCoordinatesMessage = "Invalid cell coordinates";
    private const string CellAlreadyOpenMessage = "Cell is already open";
    
    public GameResponseDto CreateGame(int width, int height, int minesCount)
    {
        var allCells = Enumerable.Range(0, height)
            .SelectMany(i => Enumerable.Range(0, width).Select(j => (i, j)))
            .ToList();

        var random = new Random();
        var shuffledCells = allCells.OrderBy(_ => random.Next()).ToList();

        var mines = new bool[height, width];
        for (var k = 0; k < minesCount; k++)
        {
            var (i, j) = shuffledCells[k];
            mines[i, j] = true;
        }

        var adjacent = new int[height, width];
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (mines[i, j])
                {
                    adjacent[i, j] = -1;
                    continue;
                }

                var count = 0;
                for (var di = -1; di <= 1; di++)
                {
                    for (var dj = -1; dj <= 1; dj++)
                    {
                        if (di == 0 && dj == 0)
                        {
                            continue;
                        }

                        var ni = i + di;
                        var nj = j + dj;

                        if (ni >= 0 && ni < height && nj >= 0 && nj < width)
                        {
                            if (mines[ni, nj])
                            {
                                count++;
                            }
                        }
                    }
                }
                adjacent[i, j] = count;
            }
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Width = width,
            Height = height,
            MinesCount = minesCount,
            Mines = mines,
            AdjacentMines = adjacent,
            Opened = new bool[height, width],
            OpenedCount = 0,
            Status = GameStatus.Active
        };

        repository.Add(game);
        return GameMapper.MapToResponse(game);
    }

    public GameResponseDto MakeTurn(Guid gameId, int row, int col)
    {
        var game = repository.GetById(gameId);

        if (game.Status != GameStatus.Active)
        {
            throw new ApiException(GameCompletedMessage);
        }

        if (row < 0 || row >= game.Height || col < 0 || col >= game.Width)
        {
            throw new ApiException(InvalidCellCoordinatesMessage);
        }

        if (game.Opened[row, col])
        {
            throw new ApiException(CellAlreadyOpenMessage);
        }

        if (game.Mines[row, col])
        {
            game.Status = GameStatus.Lose;
            repository.Update(game);
            return GameMapper.MapToResponse(game);
        }

        OpenCells(game, row, col);

        if (game.OpenedCount == game.Width * game.Height - game.MinesCount)
        {
            game.Status = GameStatus.Win;
        }

        repository.Update(game);
        return GameMapper.MapToResponse(game);
    }

    private void OpenCells(Game game, int startRow, int startCol)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((startRow, startCol));

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();

            if (game.Opened[row, col] || game.Mines[row, col])
            {
                continue;
            }

            game.Opened[row, col] = true;
            game.OpenedCount++;

            if (game.AdjacentMines[row, col] == 0)
            {
                foreach (var (dr, dc) in new[]
                         {
                             (-1, -1), (-1, 0), (-1, 1),
                             (0, -1), (0, 1),
                             (1, -1), (1, 0), (1, 1)
                         })
                {
                    var newRow = row + dr;
                    var newCol = col + dc;

                    if (newRow >= 0 && newRow < game.Height &&
                        newCol >= 0 && newCol < game.Width)
                    {
                        queue.Enqueue((newRow, newCol));
                    }
                }
            }
        }
    }
}