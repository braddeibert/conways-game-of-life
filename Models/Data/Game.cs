namespace game_of_life.Models.Data;

public record Game
{
  public required int GameId { get; set; }
  public required DateTime CreatedAt { get; set; }
  public required DateTime UpdatedAt { get; set; }
  public IList<GameCell> Cells { get; set; } = new List<GameCell>();
}