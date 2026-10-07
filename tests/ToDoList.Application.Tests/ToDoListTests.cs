using ToDoList.Application.Features.Queries;
using ToDoList.Domain;
using ToDoList.Infrastructure;
using ToDoList.Application.Features.Commands;

namespace ToDoList.Application.Tests
{
    public class ToDoListTests
    {
        [Fact]
        public async Task GetToDoListQueryHandler_WhenRepositoryHasItems_ReturnsMappedDtos()
        {
            var items = new List<ToDoItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Task 1", DateCreated = new DateTime(2026, 1, 1) },
                new() { Id = Guid.NewGuid(), Title = "Task 2", DateCreated = new DateTime(2026, 1, 2) }
            };
            var repo = new MockToDoListRepository(items);
            var handler = new GetToDoListQueryHandler(repo);

            var result = await handler.Handle(new GetToDoListQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal(items[0].Id, result[0].Id);
            Assert.Equal(items[0].Title, result[0].Title);
            Assert.Equal(items[0].DateCreated, result[0].DateCreated);
            Assert.Equal(items[1].Id, result[1].Id);
            Assert.Equal(items[1].Title, result[1].Title);
            Assert.Equal(items[1].DateCreated, result[1].DateCreated);
        }

        [Fact]
        public async Task GetToDoListQueryHandler_WhenRepositoryHasNoItems_ReturnsEmptyList()
        {
            var repo = new MockToDoListRepository([]);
            var handler = new GetToDoListQueryHandler(repo);

            var result = await handler.Handle(new GetToDoListQuery(), CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AddToDoHandler_ReturnsAddedItem()
        {
            var items = new List<ToDoItem>();
            var repo = new MockToDoListRepository(items);
            var handler = new AddToDoCommandHandler(repo);
            var result = await handler.Handle(new AddToDoCommand("New Task"), CancellationToken.None);
            Assert.Single(items);
            Assert.Equal(result.Id, items[0].Id);
            Assert.Equal("New Task", items[0].Title);
        }

        [Fact]
        public async Task AddToDoHandler_ShouldThrowForEmptyString()
        {
            var items = new List<ToDoItem>();
            var repo = new MockToDoListRepository(items);
            var handler = new AddToDoCommandHandler(repo);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(new AddToDoCommand(string.Empty), CancellationToken.None);
            });
            Assert.Empty(items);
        }

        [Fact]
        public async Task DeleteTodoHandler_ShouldReturnTrueWhenItemExists()
        {
            var items = new List<ToDoItem>();
            var repository = new MockToDoListRepository(items);
            var created = await repository.AddToDoItemAsync("to remove", CancellationToken.None);
            var handler = new DeleteToDoItemCommandHandler(repository);

            var deleted = await handler.Handle(new DeleteToDoItemCommand(created.Id), CancellationToken.None);

            Assert.True(deleted);
        }

        [Fact]
        public async Task DeleteTodoHandler_ShouldReturnFalseWhenItemDoesNotExist()
        {
            var items = new List<ToDoItem>();
            var repository = new MockToDoListRepository(items);
            var handler = new DeleteToDoItemCommandHandler(repository);

            var deleted = await handler.Handle(new DeleteToDoItemCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(deleted);
        }

        private sealed class MockToDoListRepository : IToDoListRepository
        {
            private readonly List<ToDoItem> _items;

            public MockToDoListRepository(List<ToDoItem> items)
            {
                _items = items;
            }

            public Task<List<ToDoItem>> GetToDoItemsAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_items);
            }

            public Task<ToDoItem> AddToDoItemAsync(string title, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException("Title cannot be empty.", nameof(title));
                }

                var item = new ToDoItem
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    DateCreated = DateTime.UtcNow
                };

                _items.Add(item);

                return Task.FromResult(item);
            }

            public Task<bool> DeleteToDoItemAsync(Guid id, CancellationToken cancellationToken)
            {
                var item = _items.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return Task.FromResult(false);
                }
                _items.Remove(item);
                return Task.FromResult(true);
            }
        }
    }
}
