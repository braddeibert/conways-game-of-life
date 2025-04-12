
using game_of_life.Repository;
using game_of_life.Models.API;
using game_of_life.Models.Service;
using game_of_life.Models.Data;

namespace game_of_life.Services;

public interface IGameService
{
  Task<int> CreateGameAsync(CreateGameRequest gameRequest);
  Task<GameBoard?> GetGameAsync(int gameId);
  GameBoard GetNextGeneration(GameBoard gameBoard);
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

  public async Task<GameBoard?> GetGameAsync(int gameId)
  {
    // Logic to retrieve a game by its ID
    var game = await _gameRepository.GetGameAsync(gameId);

    if (game == null)
    {
      _logger.LogWarning("Game with ID {GameId} not found.", gameId);
      return null;
    }

    var gameBoard = BuildGameBoard(game);

    return gameBoard;
  }

  public GameBoard GetNextGeneration(GameBoard gameBoard)
  {
    throw new NotImplementedException("TODO");
  }

  private GameBoard BuildGameBoard(Models.Data.Game game)
  {
    var gameBoard = new GameBoard()
    {
      GameId = game.GameId,
      Board = new List<List<int>>()
    };

    // assuming a 100x100 board
    for (int column = 0; column < 100; column++)
    {
      // create a new row
      gameBoard.Board.Add(new List<int>(100));

      // iterate through row and set cells alive from database
      for (int row = 0; row < 100; row++)
      {
        gameBoard.Board[column].Add(
          game.Cells.Any(cell => cell.X == column && cell.Y == row) ? 1 : 0
        );
      }
    }

    return gameBoard;
  }
}