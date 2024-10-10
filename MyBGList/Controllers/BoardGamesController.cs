using Microsoft.AspNetCore.Mvc;
using MyBGList.Model;

namespace MyBGList.Controllers;

[ApiController]
[Route("api/board-games")]
public class BoardGamesController(ILogger<BoardGamesController> logger) : ControllerBase
{
    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public IEnumerable<BoardGame> GetBoardGames()
    {
        return
        [
            new BoardGame() {
                Id = 1,
                Name = "Axis & Allies",
                Year = 1981,
                MinPlayers = 2,
                MaxPlayers = 5
            },
            new BoardGame() {
                Id = 2,
                Name = "Citadels",
                Year = 2000,
                MinPlayers = 2,
                MaxPlayers = 8
            },
            new BoardGame() {
                Id = 3,
                Name = "Terraforming Mars",
                Year = 2016,
                MinPlayers = 1,
                MaxPlayers = 5
            }
        ];
    }
}