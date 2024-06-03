using GameStore.Api.Entities;

namespace GameStore.Api;

public class Game
{
    public int Id { get; set; }
    public required String Name { get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public  decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }

}
