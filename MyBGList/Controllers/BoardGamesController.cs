using Microsoft.AspNetCore.Mvc;
using MyBGList.DTO;
using MyBGList.Models;

namespace MyBGList.Controllers;

[ApiController]
[Route("api/board-games")]
public class BoardGamesController(ILogger<BoardGamesController> logger) : ControllerBase
{
    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 120)]
    public RestDTO<BoardGame[]> GetBoardGames()
    {
        var data = new BoardGame[] {
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
        };

        var links = new List<LinkDTO>
        {
            new LinkDTO(
                Url.Action(null, "BoardGames", null, Request.Scheme)!,
                "self",
                "GET"),
        };
        
        return new RestDTO<BoardGame[]>
        {
            Data = data,
            Links = links
        };
    }
}