namespace game_of_life.Models.Data;

public record GameCell
{
  public required int GameCellId { get; set; }
  public required int GameId { get; set; }
  public required int X { get; set; }
  public required int Y { get; set; }
}