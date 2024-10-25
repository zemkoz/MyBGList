using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO;
using MyBGList.Models;

namespace MyBGList.Controllers;

[ApiController]
[Route("api/board-games")]
public class BoardGamesController(
    ILogger<BoardGamesController> logger, 
    ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 120)]
    public async Task<RestDTO<BoardGame[]>> GetBoardGames(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string filterQuery = "")
    {
        var query = dbContext.BoardGames.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            query = query.Where(b => b.Name.Contains(filterQuery));
        }
        
        var boardGamesArray = await query
            .OrderBy(b => b.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToArrayAsync();

        var recordCount = await query.CountAsync();
        
        var links = new List<LinkDTO>
        {
            new LinkDTO(
                Url.Action(null, "BoardGames", null, Request.Scheme)!,
                "self",
                "GET"),
        };
        
        return new RestDTO<BoardGame[]>
        {
            Data = boardGamesArray,
            Links = links,
            PageIndex = pageIndex,
            PageSize = pageSize,
            RecordCount = recordCount
        };
    }
}