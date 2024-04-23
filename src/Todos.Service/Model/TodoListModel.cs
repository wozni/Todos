public class TodoListModel
{
    public TodoListModel(ToDoList list)
    {
        Name = list.Name;
        Description = list.Description;
        ItemsCount = list.Items.Count;
    }
    
    public string Name { get; }
    public string Description { get; set; }
    public int ItemsCount { get;}
}