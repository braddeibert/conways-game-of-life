

namespace game_of_life.Models.Service;

public record GameBoard
{
  public required int GameId { get; set; }
  public List<List<int>> Board { get; set; }
}