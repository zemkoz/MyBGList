using Microsoft.AspNetCore.Mvc;
using MyBGList.Model;

namespace MyBGList.Controllers;

[ApiController]
[Route("api/board-games")]
public class BoardGamesController(ILogger<BoardGamesController> logger) : ControllerBase
{
    [HttpGet(Name = "GetBoardGames")]
    public IEnumerable<BoardGame> GetBoardGames()
    {
        return
        [
            new BoardGame() {
                Id = 1,
                Name = "Axis & Allies",
                Year = 1981
            },
            new BoardGame() {
                Id = 2,
                Name = "Citadels",
                Year = 2000
            },
            new BoardGame() {
                Id = 3,
 
                Name = "Terraforming Mars",
                Year = 2016
            }
        ];
    }
}