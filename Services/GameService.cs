
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
  GameBoard GetGameBoardGeneration(GameBoard gameBoard, int generationNumber);
}

public class GameService : IGameService
{
  private readonly ILogger<GameService> _logger;
  private readonly IGameRepository _gameRepository;

  private readonly int _boardSize = 100;

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
    var currentGeneration = gameBoard.Board;
    var nextGeneration = new List<List<int>>();

    // Assuming a 100x100 board
    for (int column = 0; column < _boardSize; column++)
    {
      // create a new row
      nextGeneration.Add(new List<int>(_boardSize));

      for (int row = 0; row < _boardSize; row++)
      {
        var isAlive = ComputeCellLife(column, row, currentGeneration);
        nextGeneration[column].Add(isAlive ? 1 : 0);
      }
    }

    var newBoard = new GameBoard
    {
      GameId = gameBoard.GameId,
      Board = nextGeneration
    };

    return newBoard;
  }

  public GameBoard GetGameBoardGeneration(GameBoard gameBoard, int generationNumber)
  {
    if (generationNumber == 0)
    {
      return gameBoard;
    }

    GameBoard nextGeneration = GetNextGeneration(gameBoard);

    // Logic to get the game board for a specific generation
    for (int i = 1; i < generationNumber; i++)
    {
      nextGeneration = GetNextGeneration(nextGeneration);
    }

    return nextGeneration;
  }

  private GameBoard BuildGameBoard(Models.Data.Game game)
  {
    var gameBoard = new GameBoard()
    {
      GameId = game.GameId,
      Board = new List<List<int>>()
    };

    // assuming a 100x100 board, fill with dead cells
    for (int column = 0; column < _boardSize; column++)
    {
      // create a new row
      gameBoard.Board.Add(new List<int>(_boardSize));

      for (int row = 0; row < _boardSize; row++)
      {
        gameBoard.Board[column].Add(0);
      }
    }

    // set alive cells from database
    foreach (var cell in game.Cells)
    {
      gameBoard.Board[cell.X][cell.Y] = 1;
    }

    return gameBoard;
  }

  private bool ComputeCellLife(int x, int y, List<List<int>> board)
  {
    var isAlive = board[x][y] == 1;
    var aliveNeighborCount = 0;

    // iterate 3x3 grid around the cell
    for (int row = x - 1; row <= x + 1; row++)
    {
      for (int column = y - 1; column <= y + 1; column++)
      {
        // avoid out of bounds errors
        if (row < 0 || row >= _boardSize || column < 0 || column >= _boardSize)
        {
          continue;
        }

        var isSelf = row == x && column == y;
        if (isSelf)
        {
          continue;
        }

        // checking if neighbor is alive
        if (board[row][column] == 1)
        {
          aliveNeighborCount++;
        }
      }
    }

    // Apply the rules of Conway's Game of Life
    // dead cell with 3 alive neighbors becomes alive
    if (!isAlive && aliveNeighborCount == 3) return true;

    // living cells with 2 or 3 neighbors stay alive
    if (isAlive && (aliveNeighborCount == 2 || aliveNeighborCount == 3)) return true;

    // living cells with less than 2 or more than 3 neighbors die
    // dead cells without exactly 3 neighbors stay dead
    return false;
  }
}