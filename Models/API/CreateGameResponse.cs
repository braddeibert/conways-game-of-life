

using System.Text.Json.Serialization;

namespace game_of_life.Models.API;

public record CreateGameResponse
{
  [JsonPropertyName("GameId")]
  [JsonRequired]
  public int GameId { get; init; }

  [JsonPropertyName("Message")]
  public string Message { get; init; }
}
