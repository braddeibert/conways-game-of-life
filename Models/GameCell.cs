

namespace game_of_life.Models;

public record GameCell
{
  public required int X { get; set; }
  public required int Y { get; set; }
  public bool IsAlive { get; set; } = false;
}