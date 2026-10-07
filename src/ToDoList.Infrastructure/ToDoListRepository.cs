using System.Collections.Concurrent;
using ToDoList.Domain;

namespace ToDoList.Infrastructure
{
    public sealed class ToDoListRepository : IToDoListRepository
    {
        private readonly List<ToDoItem> _toDoItems = new List<ToDoItem>() { 
            new ToDoItem { Id = Guid.NewGuid(), Title = "Sample Task", DateCreated = DateTime.UtcNow }, 
            new ToDoItem { Id = Guid.NewGuid(), Title = "Another Task", DateCreated = DateTime.UtcNow } 
        };

        public Task<List<ToDoItem>> GetToDoItemsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_toDoItems);
        }

        public Task<ToDoItem> AddToDoItemAsync(string title, CancellationToken cancellationToken)
        {
            var newItem = new ToDoItem { Id = Guid.NewGuid(), Title = title, DateCreated = DateTime.UtcNow };
            _toDoItems.Add(newItem);
            return Task.FromResult(newItem);
        }

        public Task<bool> DeleteToDoItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = _toDoItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return Task.FromResult(false);
            }
            _toDoItems.Remove(item);
            return Task.FromResult(true);
        }
    }
}
