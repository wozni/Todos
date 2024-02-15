using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Service.Application;
using TodoList.Service.Model;

var builder = WebApplication.CreateBuilder();
// Rejestracja usług w kontenerze
builder.Services.AddSqlServer<ApplicationContext>(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddLogging();
// Tworzymy obiekt aplikacji webowej z usługami 
var app = builder.Build();
// Konfigurujemy middleware
    
app.MapGet("/notebooks", async ([FromServices] ILogger<Notebook> logger, [FromServices] ApplicationContext db) =>
{
    return await db.Notebooks
        .Include(x => x.Notes)
        .ToListAsync();
});
app.MapGet("/notebooks/{id:int}", async ([FromServices] ApplicationContext db, int id) =>
{
    return await db.Notebooks
        .Include(x => x.Notes)
        .FirstOrDefaultAsync(notebook => notebook.Id == id);
});
app.MapPost("/notebooks", async ([FromBody]AddNotebookRequest request, [FromServices] ApplicationContext db) =>
{
    var notebook = new Notebook
    {
        Title = request.Title,
        Description = request.Description
    };
    db.Notebooks.Add(notebook);
    await db.SaveChangesAsync(); 
    return $"Added notebook with id: {notebook.Id}";
});

app.MapPost("/notebooks/{notebookId:int}/notes", async ([FromServices]ApplicationContext db,  AddNotebookNodeRequest request, int notebookId) =>
{
    var notebook = await db.Notebooks.FirstOrDefaultAsync(x => x.Id == notebookId);
    if (notebook != null)
    {
        var result = notebook.AddNote(request);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Error);
        }
        await db.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});

app.MapDelete("/notebooks/{notebookId:int}", () =>
{
   // TODO
});

app.MapDelete("/notebooks/{notebookId:int}/notes/{noteId:int}", () =>
{
    // TODO
});



using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.MigrateAsync();

// uruchamiamy aplikację
await app.RunAsync();
