
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace game_of_life.Models.API;

public record CellCoordinates
{
  [JsonPropertyName("x")]
  [JsonRequired]
  [Range(0, 100)]
  public int X { get; init; }

  [JsonPropertyName("y")]
  [JsonRequired]
  [Range(0, 100)]
  public int Y { get; init; }
}