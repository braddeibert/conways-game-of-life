using Microsoft.AspNetCore.Mvc;
using game_of_life.Models.API;
using game_of_life.Services;
using System.Threading.Tasks;

namespace game_of_life.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameService _gameService;

    public GameController(ILogger<GameController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    // POST /api/game/create
    /// <summary>
    /// Creates a new game from an input seed.
    /// /// </summary>
    /// <param name="game">The game object containing the seed and other parameters.</param>
    /// <returns>Returns the ID of the created game board.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest game)
    {
        try
        {
            var gameId = await _gameService.CreateGameAsync(game);

            var response = new CreateGameResponse
            {
                GameId = gameId,
                Message = "Game created successfully."
            };

            // return 201 Created with the game ID
            _logger.LogInformation("Game created successfully with ID: {GameId}", gameId);
            return StatusCode(201, response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid game creation request: {Message}", ex.Message);

            var errorResponse = new ErrorResponse
            {
                Error = "InvalidGameError",
                Message = ex.Message
            };

            return BadRequest(errorResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating game: {Message}", ex.Message);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // GET /api/game/{id}/next-generation
    /// <summary>
    /// Gets the next generation of the game board by ID.
    /// /// </summary>
    /// <param name="id">The ID of the game board.</param>
    /// <returns>Returns the next generation of the game board.</returns>
    [HttpGet("{id}/next-generation")]
    public async Task<IActionResult> GetNextGeneration(int id)
    {
        var gameBoard = await _gameService.GetGameAsync(id);
        if (gameBoard == null)
        {
            var errorResponse = new ErrorResponse
            {
                Error = "GameNotFound",
                Message = $"Game with ID {id} does not exist."
            };

            return NotFound(errorResponse);
        }

        var nextGeneration = _gameService.GetNextGeneration(gameBoard);

        return Ok(nextGeneration);
    }

    // GET /api/game/{id}/generation-number/{generationNumber}
    /// <summary>
    /// Gets the n-th generation of a game board.
    /// /// </summary>
    /// <param name="id">The ID of the game board.</param>
    /// <param name="generationNumber">The generation number to retrieve.</param>
    /// <returns>Returns the n-th generation of the game board.</returns>
    [HttpGet("{id}/generation-number/{generationNumber}")]
    public IActionResult GetGeneration(int id, int generationNumber)
    {
        // Logic to get the specified generation of the game board
        // ...

        return Ok("Generation retrieved successfully.");
    }

    // GET /api/game/{id}/final-generation
    /// <summary>
    /// Gets the final generation of a game board by ID.
    /// /// </summary>
    /// <param name="id">The ID of the game board.</param>
    /// <returns>Returns the final generation of the game board if possible. Otherwise returns an error.</returns>
    [HttpGet("{id}/final-generation")]
    public IActionResult GetFinalGeneration(int id)
    {
        // Logic to get the final generation of the game board
        // ...

        return Ok("Final generation retrieved successfully.");
    }
}
