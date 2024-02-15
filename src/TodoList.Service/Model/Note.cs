using System.Text.Json.Serialization;

namespace TodoList.Service.Model;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    [JsonIgnore]
    public Notebook Owner { get; set; }
}
