using TodoList.Service.Model;

public class Notebook
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public List<Note> Notes { get; } = new();

    // TODO: Nie więcej niż 10 notatek
    // TODO: Tytuł nie dłuższy niż 50 znaków (xunit - testy z parametrami, Theory)
    
    public AddNotebookNodeResponse AddNote(AddNotebookNodeRequest request)
    {
        if (string.IsNullOrEmpty(request.Title) ||
            string.IsNullOrEmpty(request.Content))
        {
            return new AddNotebookNodeResponse
            {
                Succeeded = false,
                Error = "Title or content cannot be empty"
            };
        }
        
        Notes.Add(new Note
        {
            Title = request.Title,
            Content = request.Content
        });

        return new AddNotebookNodeResponse
        {
            Succeeded = true
        };
    }
}
