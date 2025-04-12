

using game_of_life.Models.API;

namespace game_of_life.Services;

public interface IGameService
{
  Task<int> CreateGameAsync(CreateGameRequest gameRequest);
}

public class GameService : IGameService
{
  private readonly ILogger<GameService> _logger;

  public GameService(ILogger<GameService> logger)
  {
    _logger = logger;
  }

  // Logic to create a new game
  public async Task<int> CreateGameAsync(CreateGameRequest gameRequest)
  {
    if (gameRequest.Cells == null || gameRequest.Cells.Count == 0)
    {
      _logger.LogError("Create game request is invalid: no cells provided.");
      throw new ArgumentException("Creating a new game requires at least one living cell.");
    }

    // TODO: create board and cells in database


    return 1; // Return the ID of the created game board
  }

}