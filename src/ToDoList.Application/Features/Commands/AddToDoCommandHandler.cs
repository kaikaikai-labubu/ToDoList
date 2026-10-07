using MediatR;
using ToDoList.Application.Features.Dtos;
using ToDoList.Infrastructure;

namespace ToDoList.Application.Features.Commands
{
    public sealed record AddToDoCommand(string Title) : IRequest<ToDoItemDto>;

    public sealed class AddToDoCommandHandler : IRequestHandler<AddToDoCommand, ToDoItemDto>
    {
        private readonly IToDoListRepository _repo;

        public AddToDoCommandHandler(IToDoListRepository repo)
        {
            _repo = repo;
        }

        public async Task<ToDoItemDto> Handle(AddToDoCommand request, CancellationToken cancellationToken)
        {
            var toDoItem = await _repo.AddToDoItemAsync(request.Title, cancellationToken);
            return new ToDoItemDto
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                DateCreated = toDoItem.DateCreated  
            };
        }
    }
}
