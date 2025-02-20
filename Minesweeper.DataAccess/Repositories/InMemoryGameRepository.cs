using System.Collections.Concurrent;
using Minesweeper.Domain.Entities;
using Minesweeper.Domain.Exceptions;
using Minesweeper.Domain.Interfaces;

namespace Minesweeper.Infrastructure.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public void Add(Game game)
    {
        if (!_games.TryAdd(game.Id, game))
        {
            throw new ApiException($"Game with ID {game.Id} already exists");
        }        
    }

    public Game GetById(Guid id)
    {
        if (!_games.TryGetValue(id, out var game))
        {
            throw new ApiException($"Game with ID {id} not found");
        }        
        
        return game;
    }

    public void Update(Game game)
    {
        if (!_games.ContainsKey(game.Id))
        {
            throw new ApiException($"Game with ID {game.Id} not found");
        }

        _games[game.Id] = game;
    }
}