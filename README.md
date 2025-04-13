# conways-game-of-life
Conway's Game of Life implemented with a .NET API and SQLite database.

## Limitations
- The game board is finite, and bounded to 100x100 squares.
- Only initial board state is persisted to database. This means all game board generations are computed per API request. (A good performance improvement could be caching every 100-1000 generations.)
- The "Get final state" endpoint has not been implemented.

## Run Locally
Note: Running locally will automatically create a SQLite database for the application in memory using Entity Framework tools.
1. `dotnet run`

## API Endpoints
Note: There is a Postman collection provided you can import & use: `conways-game-api.postman_collection.json`

### Create a New Game
Creates a new game with initial live cells configuration.

**Endpoint:** `POST /api/game/create`

**Request Body:**
```json
{
  "Cells": [
    { "x": 10, "y": 10 },
    { "x": 10, "y": 11 },
    { "x": 10, "y": 12 }
  ]
}
```

**Response:**
```json
{
  "GameId": 1,
  "Message": "Game created successfully."
}
```

**Status Codes:**
- `201 Created`: Game created successfully
- `400 Bad Request`: Invalid request format or data
- `500 Internal Server Error`: Server-side error

### Get Next Generation
Computes and returns the next generation of a game board.

**Endpoint:** `GET /api/game/{id}/next-generation`

**Path Parameters:**
- `id`: ID of the game

**Response:**
```json
{
  "GameId": 1,
  "IsStill": false,
  "IsDead": false,
  "Board": [[0,0,0,...], [0,1,1,...], ...]
}
```

**Status Codes:**
- `200 OK`: Successfully returned next generation
- `404 Not Found`: Game ID not found

### Get Specific Generation
Computes and returns a specific generation of a game board.

**Endpoint:** `GET /api/game/{id}/generation-number/{generationNumber}`

**Path Parameters:**
- `id`: ID of the game
- `generationNumber`: Generation number to compute (0 is initial state)

**Response:**
```json
{
  "GameId": 1,
  "IsStill": false,
  "IsDead": false,
  "Board": [[0,0,0,...], [0,1,1,...], ...]
}
```

**Status Codes:**
- `200 OK`: Successfully returned requested generation
- `404 Not Found`: Game ID not found

