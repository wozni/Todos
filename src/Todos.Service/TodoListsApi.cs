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
            var newList = new ToDoList
            {
                Name = model.Name,
                Description = model.Description
            };
            context.TodoLists.Add(newList);
            await context.SaveChangesAsync();
            return $"Utworzono nową listę o nazwie: {newList.Name}";
        });
    }
}
