
using game_of_life.Repository;
using game_of_life.Models.API;

namespace game_of_life.Services;

public interface IGameService
{
  Task<int> CreateGameAsync(CreateGameRequest gameRequest);
}

public class GameService : IGameService
{
  private readonly ILogger<GameService> _logger;
  private readonly IGameRepository _gameRepository;

  public GameService(ILogger<GameService> logger, IGameRepository gameRepository)
  {
    _logger = logger;
    _gameRepository = gameRepository;
  }

  // Logic to create a new game
  public async Task<int> CreateGameAsync(CreateGameRequest gameRequest)
  {
    if (gameRequest.Cells == null || gameRequest.Cells.Count == 0)
    {
      _logger.LogError("Create game request is invalid: no cells provided.");
      throw new ArgumentException("Creating a new game requires at least one living cell.");
    }

    var gameCells = gameRequest.Cells
      .Select(cell => new Models.Service.GameCell
      {
        X = cell.X,
        Y = cell.Y,
        IsAlive = true // All cells provided in the request are alive
      })
      .ToList();

    // create game & cells in database
    var game = await _gameRepository.CreateGameAsync(gameCells);

    return game.GameId; // Return the ID of the created game board
  }

}