using Microsoft.AspNetCore.Mvc;

namespace game_of_life.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // POST /api/game/create
    /// <summary>
    /// Creates a new game from an input seed.
    /// /// </summary>
    /// <param name="game">The game object containing the seed and other parameters.</param>
    /// <returns>Returns the ID of the created game board.</returns>
    [HttpPost("create")]
    public IActionResult CreateGame([FromBody] Game game)
    {
        // Logic to create a new game
        // ...

        return Ok("Game created successfully.");
    }

    // GET /api/game/{id}/next-generation
    /// <summary>
    /// Gets the next generation of the game board by ID.
    /// /// </summary>
    /// <param name="id">The ID of the game board.</param>
    /// <returns>Returns the next generation of the game board.</returns>
    [HttpGet("{id}/next-generation")]
    public IActionResult GetNextGeneration(int id)
    {
        // Logic to get the next generation of the game board
        // ...

        return Ok("Next generation retrieved successfully.");
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
