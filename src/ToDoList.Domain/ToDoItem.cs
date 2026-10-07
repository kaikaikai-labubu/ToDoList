namespace ToDoList.Domain
{
    public sealed class ToDoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}