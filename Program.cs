using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddNpgsql<GameStoreContext>(connString);

var app = builder.Build();


app.MapGameEndPoints();
app.MapsGenresEndPoints();

await app.MigrateDbAsync();
app.Run();
