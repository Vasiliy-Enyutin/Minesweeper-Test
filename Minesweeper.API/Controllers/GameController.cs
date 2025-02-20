using Microsoft.AspNetCore.Mvc;
using Minesweeper.Application.DTOs;
using Minesweeper.Application.Services;

namespace Minesweeper.API.Controllers;

[ApiController]
[Route("api")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpPost("new")]
    public IActionResult CreateGame([FromBody] NewGameRequestDto requestDto)
    {
        return Ok(gameService.CreateGame(requestDto.Width, requestDto.Height, requestDto.MinesCount));
    }

    [HttpPost("turn")]
    public IActionResult MakeTurn([FromBody] TurnRequestDto requestDto)
    {
        return Ok(gameService.MakeTurn(requestDto.GameId, requestDto.Row, requestDto.Col));
    }
}