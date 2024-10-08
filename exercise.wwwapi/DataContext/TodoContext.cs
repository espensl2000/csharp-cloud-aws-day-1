using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace exercise.wwwapi.DataContext
{
  public class TodoContext : DbContext
  {
    private static string GetConnectionString()
    {
      string jsonSettings = File.ReadAllText("appsettings.json");
      JObject configuration = JObject.Parse(jsonSettings);
      return configuration["ConnectionStrings"]["DefaultConnection"].ToString();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Todo>().HasData(
            new Todo { Id = 1, Title = "First task", Completed = true },
            new Todo { Id = 2, Title = "Second task", Completed = false },
            new Todo { Id = 3, Title = "Third task", Completed = true }
        );
    }

    public DbSet<Todo> Todos { get; set; }
  }
}