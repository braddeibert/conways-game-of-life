
namespace game_of_life.Models.API;

public class GameBoardResponse
{
  public required int GameId { get; set; }
  public List<List<int>> Board { get; set; }
}