namespace TodoList.Service.Model;

public class AddNotebookNodeRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class AddNotebookNodeResponse
{
    public bool Succeeded { get; set; }
    public string Error { get; set; }
}
