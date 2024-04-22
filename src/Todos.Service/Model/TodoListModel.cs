public class TodoListModel
{
    public TodoListModel(ToDoList list)
    {
        Name = list.Name;
        ItemsCount = list.Items.Count;
    }
    
    public string Name { get; }
    public int ItemsCount { get;}
}