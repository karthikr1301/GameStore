using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore; 
namespace GameStore.Api.Endpoints;


public static class GenreEndPoints
{
    public static RouteGroupBuilder MapsGenresEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async(GameStoreContext DbContext) => 
            await DbContext.Genres
                            .Select(genre => genre.ToDto())
                            .AsNoTracking()
                            .ToListAsync());

        return group;
    }

}
