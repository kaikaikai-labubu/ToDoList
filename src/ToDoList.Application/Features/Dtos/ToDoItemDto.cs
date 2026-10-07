namespace ToDoList.Application.Features.Dtos
{
    public class ToDoItemDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
