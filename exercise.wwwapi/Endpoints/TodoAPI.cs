using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;

namespace exercise.wwwapi.EndPoints
{
  public static class TodoAPI
  {
    public static void ConfigureTodoAPI(this WebApplication app)
    {
            app.MapGet("/", GetIndex);
            app.MapGet("/todos", GetTodos);
      app.MapPost("/todos", CreateTodo);
      app.MapPut("/todos/{id:int}", UpdateTodo);
      app.MapDelete("/todos/{id:int}", DeleteTodo);
    }

    private static async Task<IResult> GetIndex(IDatabaseRepository<Todo> repository)
    {
        try
        {
            return Results.Ok("Working");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetTodos(IDatabaseRepository<Todo> repository)
    {
      try
      {
        return Results.Ok(repository.GetAll());
      }
      catch (Exception ex)
      {
        return Results.Problem(ex.Message);
      }
    }

    private static async Task<IResult> CreateTodo(Todo todo, IDatabaseRepository<Todo> repository)
    {
      try
      {
        if (todo == null || string.IsNullOrWhiteSpace(todo.Title))
        {
          return Results.BadRequest("Invalid Todo item.");
        }

        repository.Insert(todo); // Assuming repository has an AddAsync method
        return Results.Created($"/todos/{todo.Id}", todo); // Return the created todo with its location
      }
      catch (Exception ex)
      {
        return Results.Problem(ex.Message);
      }
    }

    private static async Task<IResult> UpdateTodo(int id, Todo updatedTodo, IDatabaseRepository<Todo> repository)
    {
      try
      {
        // Find the existing Todo
        var existingTodo = repository.GetById(id);
        if (existingTodo == null)
        {
          return Results.NotFound($"Todo with Id {id} not found.");
        }

        // Update the fields
        if (updatedTodo.Title != null)
          existingTodo.Title = updatedTodo.Title;
        if (updatedTodo.Completed != null)
          existingTodo.Completed = updatedTodo.Completed;

        // Save changes
        repository.Update(existingTodo);

        return Results.Ok(existingTodo);
      }
      catch (Exception ex)
      {
        return Results.Problem(ex.Message);
      }
    }

    private static async Task<IResult> DeleteTodo(int id, IDatabaseRepository<Todo> repository)
    {
      try
      {
        // Find the Todo by Id
        var existingTodo = repository.GetById(id);
        if (existingTodo == null)
        {
          return Results.NotFound($"Todo with Id {id} not found.");
        }

        // Delete the Todo
        repository.Delete(id);

        return Results.Ok(existingTodo);
      }
      catch (Exception ex)
      {
        return Results.Problem(ex.Message);
      }
    }
  }
}