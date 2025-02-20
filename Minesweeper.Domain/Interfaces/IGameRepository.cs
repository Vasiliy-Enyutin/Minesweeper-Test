using Minesweeper.Domain.Entities;

namespace Minesweeper.Domain.Interfaces;

public interface IGameRepository
{
    void Add(Game game);
    Game GetById(Guid id);
    void Update(Game game);
}