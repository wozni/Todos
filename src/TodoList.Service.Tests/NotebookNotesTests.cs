using TodoList.Service.Model;

namespace TodoList.Service.Tests;

public class NotebookNotesTests
{
    
    [Fact]
    public void Should_deny_adding_note_with_empty_title()
    {
        // Arrange
        var notebook = new Notebook();
        var request = new AddNotebookNodeRequest
        {
            Title = "",
            Content = "Dummy"
        };
        // Act
        var response = notebook.AddNote(request);
        // Assert
        Assert.False(response.Succeeded);
        Assert.Equal("Title or content cannot be empty", response.Error);
    }
}
