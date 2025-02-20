using Minesweeper.Application.DTOs;

namespace Minesweeper.Application.Services;

public interface IGameService
{
    GameResponseDto CreateGame(int width, int height, int minesCount);
    GameResponseDto MakeTurn(Guid gameId, int row, int col);
}