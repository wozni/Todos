namespace MAVLink.Gateway.Service;

public static class TodoListsApi
{
    public static void MapTodosApi(this WebApplication app)
    {
        app.MapGet("/todos", async ([FromServices] AppContext context) =>
        {
            var lists = await context.TodoLists
                .Include(x => x.Items)
                .ToListAsync();
            return lists
                .Select(todoList => new TodoListModel(todoList));
        });

        app.MapPost("/todos", async ([FromServices] AppContext context, TodoListModel model) =>
        {
            var existingList = await context.TodoLists.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (existingList != null) return $"Lista o nazwie '{model.Name}' już istnieje.";

            var newList = new ToDoList
            {
                Name = model.Name,
                Description = model.Description
            };
            context.TodoLists.Add(newList);
            await context.SaveChangesAsync();
            return $"Utworzono nową listę o nazwie: {newList.Name}";
        });

        app.MapDelete("/todos/{name}", async ([FromServices] AppContext context, string name) =>
        {
            var listToDelete = await context.TodoLists.FirstOrDefaultAsync(t => t.Name == name);
            if (listToDelete == null) return $"Lista o nazwie {name} nie istnieje";
            else
            {
                context.TodoLists.Remove(listToDelete);
                await context.SaveChangesAsync();
                return $"Usunięto listę o nazwie: {name}";
            }
        });
    }
}
