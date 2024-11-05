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
    
    [HttpPost(Name = "UpdateBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult<RestDTO<BoardGame?>>> Post(BoardGameDTO model)
    {
        var boardgame = await dbContext.BoardGames
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        
        if (boardgame == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(model.Name))
        {
            boardgame.Name = model.Name;
        }

        if (model.Year is > 0)
        {
            boardgame.Year = model.Year.Value;
        }

        boardgame.LastModifiedDate = DateTime.Now;
        dbContext.BoardGames.Update(boardgame);
        await dbContext.SaveChangesAsync();

        var restDto = new RestDTO<BoardGame?>()
        {
            Data = boardgame,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "BoardGames",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST"),
            }
        };
        
        return Ok(restDto);
    }
    
    [HttpDelete(Name = "DeleteBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult<RestDTO<BoardGame?>>> Delete(int id)
    {
        var boardgame = await dbContext.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (boardgame == null)
        {
            return NotFound();
        }
        
        dbContext.BoardGames.Remove(boardgame);
        await dbContext.SaveChangesAsync();

        var restDto = new RestDTO<BoardGame?>()
        {
            Data = boardgame,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "BoardGames",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE"),
            }
        };

        return Ok(restDto);
    }
}