using ToDoList.Domain;

namespace ToDoList.Infrastructure
{
    public interface IToDoListRepository
    {
        Task<List<ToDoItem>> GetToDoItemsAsync(CancellationToken cancellationToken);

        Task<ToDoItem> AddToDoItemAsync(string title, CancellationToken cancellationToken);

        Task<bool> DeleteToDoItemAsync(Guid id, CancellationToken cancellationToken);
    }
}
