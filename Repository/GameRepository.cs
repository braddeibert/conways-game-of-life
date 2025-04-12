using game_of_life.Models.Service;
using game_of_life.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace game_of_life.Repository;

public interface IGameRepository
{
  Task<Game> CreateGameAsync(IList<Models.Service.GameCell> cells);
  Task<Game> GetGameAsync(int gameId);
}

public class GameRepository : IGameRepository
{
  private readonly GameDbContext _context;

  public GameRepository(GameDbContext context)
  {
    _context = context;
  }

  public async Task<Game> CreateGameAsync(IList<Models.Service.GameCell> cells)
  {
    if (cells == null || cells.Count == 0)
    {
      throw new ArgumentException("Game must have at least one cell.");
    }

    // Create a new game instance
    var game = new Game
    {
      GameId = 0, // This will be set by the database
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
    };

    game.Cells = cells.Select(cell => new Models.Data.GameCell
    {
      GameCellId = 0, // This will be set by the database
      GameId = game.GameId, // This will be set later
      X = cell.X,
      Y = cell.Y,
    }).ToList();

    // Add the game 
    _context.Games.Add(game);

    // Add each cell for game
    foreach (var cell in game.Cells)
    {
      cell.GameId = game.GameId; // link to game record
      _context.GameCells.Add(cell);
    }

    await _context.SaveChangesAsync();

    return game;
  }

  public async Task<Game?> GetGameAsync(int gameId)
  {
    // Retrieve the game by its ID
    var game = await _context.Games
      .Include(g => g.Cells)
      .FirstOrDefaultAsync(g => g.GameId == gameId);

    return game;
  }
}