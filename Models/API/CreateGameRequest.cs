using System.Text.Json.Serialization;

namespace game_of_life.Models.API;

public record CreateGameRequest
{
  [JsonPropertyName("Cells")]
  [JsonRequired]
  public IList<CellCoordinates> Cells { get; init; } = new List<CellCoordinates>();
}