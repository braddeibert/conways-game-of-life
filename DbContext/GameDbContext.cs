

using game_of_life.Models.Data;
using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
  public GameDbContext(DbContextOptions<GameDbContext> options)
      : base(options)
  {
    // database file is created in project root directory
    DbPath = Path.Join(Environment.CurrentDirectory, "game.db");

    // create database if it does not exist
    if (!File.Exists(DbPath))
    {
      Database.EnsureCreated();
    }
  }

  public string DbPath { get; }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlite($"Data Source={DbPath}");

  public DbSet<Game> Games { get; set; } = null!;
  public DbSet<GameCell> GameCells { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Game>()
      .HasKey(g => g.GameId);

    modelBuilder.Entity<GameCell>()
      .HasKey(gc => gc.GameCellId);

    modelBuilder.Entity<Game>()
      .HasMany(g => g.Cells)
      .WithOne()
      .HasForeignKey(gc => gc.GameId);
  }
}