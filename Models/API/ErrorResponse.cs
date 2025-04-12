using System.Text.Json.Serialization;

namespace game_of_life.Models.API;

public record ErrorResponse
{
  [JsonPropertyName("Error")]
  [JsonRequired]
  public string Error { get; init; } = string.Empty;

  [JsonPropertyName("Message")]
  [JsonRequired]
  public string Message { get; init; } = string.Empty;
}