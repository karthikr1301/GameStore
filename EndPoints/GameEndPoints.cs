using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Api.Endpoints
{ 
public static class GameEndPoints
{
    const string GetGameEndPointName="GetGame";
    
    public static RouteGroupBuilder MapGameEndPoints(this WebApplication app)
    {
        var group= app.MapGroup("games")
                                    .WithParameterValidation();
        // Get /games
        group.MapGet("/", async (GameStoreContext DbContext) =>
            await DbContext.Games 
                    .Include(game=>game.Genre)
                    .Select(game=>game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync());

        // Get /games

        group.MapGet("/{id}", async (int id, GameStoreContext DbContext) => 
        {
            Game? game= await DbContext.Games.FindAsync(id);
            
            return game is null ? 
            Results.NotFound(): Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndPointName);


        //POST/games

        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext DbContext) =>
        {
            Game game=newGame.ToEntity();

            DbContext.Games.Add(game);
            await DbContext.SaveChangesAsync();


            return Results.CreatedAtRoute(
                GetGameEndPointName, 
                new{id=game.Id},
                game.ToGameDetailsDto());
        });


        // Put games

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext DbContext)=>
        {
            var existingGame = await DbContext.Games.FindAsync(id);
            if(existingGame is null)
            {
                return Results.NotFound();

            }

            DbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));
            await DbContext.SaveChangesAsync();        

            /*games[index]=new GameSummaryDto(   
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );*/
           
            return Results.NoContent();

            
        });


        //Deletre/games


        group.MapDelete("/{id}", async (int id ,  GameStoreContext DbContext) =>
        {
            //games.RemoveAll(game =>game.Id==id);
            await DbContext.Games
                     .Where(game=>game.Id==id)
                     .ExecuteDeleteAsync();

            return  Results.NoContent();
        });

        return group;
    }
        




}
}
