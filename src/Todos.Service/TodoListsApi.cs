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
            if (existingList != null) return Results.BadRequest($"Lista o nazwie '{model.Name}' już istnieje.");

            var newList = new ToDoList
            {
                Name = model.Name,
                Description = model.Description
            };
            context.TodoLists.Add(newList);
            await context.SaveChangesAsync();
            return Results.Created($"/todos", $"Utworzono nową listę o nazwie: {newList.Name}");
        });

        app.MapDelete("/todos/{name}", async ([FromServices] AppContext context, string name) =>
        {
            var listToDelete = await context.TodoLists.FirstOrDefaultAsync(t => t.Name == name);
            if (listToDelete == null) return Results.BadRequest($"Lista o nazwie {name} nie istnieje");
            context.TodoLists.Remove(listToDelete);
            await context.SaveChangesAsync();
            return Results.Ok($"Usunięto listę o nazwie: {name}");
        });

        app.MapPost("/todos/{list_name}/item", async ([FromServices] AppContext context, string list_name, ToDoListItemModel model) =>
        {
            var todoList = await context.TodoLists.FirstOrDefaultAsync(t => t.Name == list_name);
            if (todoList == null) return Results.BadRequest($"Nie znaleziono listy o nazwie: {list_name}");
            var newItem = new ToDoListItem
            {
                Name = model.Name,
                Owner = todoList
            };
            todoList.Items.Add(newItem);
            await context.SaveChangesAsync();
            return Results.Created($"/todos/{list_name}/item", $"Dodano nowe zadanie do listy o nazwie: {list_name}");
        });
    }
}
