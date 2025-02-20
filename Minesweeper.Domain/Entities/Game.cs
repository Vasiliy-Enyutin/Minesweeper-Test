namespace Minesweeper.Domain.Entities;

public class Game
{
    public Guid Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
    public bool[,] Mines { get; set; } = new bool[0, 0];
    public int[,] AdjacentMines { get; set; } = new int[0, 0];
    public bool[,] Opened { get; set; } = new bool[0, 0];
    public int OpenedCount { get; set; }
    public GameStatus Status { get; set; }
}

public enum GameStatus
{
    Active,
    Win,
    Lose
}