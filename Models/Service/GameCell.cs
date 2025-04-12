namespace game_of_life.Models.Service;

public record GameCell
{
  public required int X { get; set; }
  public required int Y { get; set; }
  public required bool IsAlive { get; set; }
}