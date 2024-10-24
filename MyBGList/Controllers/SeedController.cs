using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;
using MyBGList.Models.Csv;

namespace MyBGList.Controllers;

[ApiController]
[Route("api/seed")]
public class SeedController(
    ILogger<BoardGamesController> logger,
    ApplicationDbContext dbContext,
    IHostEnvironment environment) : ControllerBase
{
    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> Put()
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
        {
            HasHeaderRecord = true,
            Delimiter = ";",
        };
        using var reader = new StreamReader(
            System.IO.Path.Combine(environment.ContentRootPath, "Data/bgg_dataset.csv"));
        using var csv = new CsvReader(reader, config);
        var existingBoardGames = await dbContext.BoardGames
            .ToDictionaryAsync(bg => bg.Id);
        var existingDomains = await dbContext.Domains
            .ToDictionaryAsync(d => d.Name);
        var existingMechanics = await dbContext.Mechanics
            .ToDictionaryAsync(m => m.Name);
        var now = DateTime.Now;

        var records = csv.GetRecords<BggRecord>();
        var skippedRows = 0;
        foreach (var record in records)
        {
            if (!record.ID.HasValue
                || string.IsNullOrEmpty(record.Name)
                || existingBoardGames.ContainsKey(record.ID.Value))
            {
                skippedRows++;
                continue;
            }

            var boardGame = new BoardGame()
            {
                Id = record.ID.Value,
                Name = record.Name,
                BGGRank = record.BGGRank ?? 0,
                ComplexityAverage = record.ComplexityAverage ?? 0,
                MaxPlayers = record.MaxPlayers ?? 0,
                MinAge = record.MinAge ?? 0,
                MinPlayers = record.MinPlayers ?? 0,
                OwnedUsers = record.OwnedUsers ?? 0,
                PlayTime = record.PlayTime ?? 0,
                RatingAverage = record.RatingAverage ?? 0,
                UsersRated = record.UsersRated ?? 0,
                Year = record.YearPublished ?? 0,
                CreatedDate = now,
                LastModifiedDate = now,
            };
            dbContext.BoardGames.Add(boardGame);

            if (!string.IsNullOrEmpty(record.Domains))
                foreach (var domainName in record.Domains
                             .Split(',', StringSplitOptions.TrimEntries)
                             .Distinct(StringComparer.InvariantCultureIgnoreCase))
                {
                    var domain = existingDomains.GetValueOrDefault(domainName);
                    if (domain == null)
                    {
                        domain = new Domain()
                        {
                            Name = domainName,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };
                        dbContext.Domains.Add(domain);
                        existingDomains.Add(domainName, domain);
                    }

                    dbContext.BoardGames2Domains.Add(new BoardGame2Domain()
                    {
                        BoardGame = boardGame,
                        Domain = domain,
                        CreatedDate = now
                    });
                }

            if (!string.IsNullOrEmpty(record.Mechanics))
                foreach (var mechanicName in record.Mechanics
                             .Split(',', StringSplitOptions.TrimEntries)
                             .Distinct(StringComparer.InvariantCultureIgnoreCase))
                {
                    var mechanic = existingMechanics.GetValueOrDefault(mechanicName);
                    if (mechanic == null)
                    {
                        mechanic = new Mechanic()
                        {
                            Name = mechanicName,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };
                        dbContext.Mechanics.Add(mechanic);
                        existingMechanics.Add(mechanicName, mechanic);
                    }

                    dbContext.BoardGames2Mechanics.Add(new BoardGame2Mechanic()
                    {
                        BoardGame = boardGame,
                        Mechanic = mechanic,
                        CreatedDate = now
                    });
                }
        }

        // SAVE
        using var transaction = dbContext.Database.BeginTransaction();
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames ON");
        await dbContext.SaveChangesAsync();
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames OFF");
        transaction.Commit();
        
        return new JsonResult(new
        {
            BoardGames = dbContext.BoardGames.Count(),
            Domains = dbContext.Domains.Count(),
            Mechanics = dbContext.Mechanics.Count(),
            SkippedRows = skippedRows
        });
    }
}