using MediatR;
using ToDoList.Application.Features.Dtos;
using ToDoList.Infrastructure;

namespace ToDoList.Application.Features.Queries
{
    public sealed record GetToDoListQuery : IRequest<IList<ToDoItemDto>>
    {
        public string? Title { get; set; }
    }

    public sealed class GetToDoListQueryHandler : IRequestHandler<GetToDoListQuery, IList<ToDoItemDto>>
    {
        private readonly IToDoListRepository _repo;

        public GetToDoListQueryHandler(IToDoListRepository repo) { 
            _repo = repo;
        }

        public async Task<IList<ToDoItemDto>> Handle(GetToDoListQuery request, CancellationToken cancellationToken)
        {
            var toDoItems = await _repo.GetToDoItemsAsync(cancellationToken);
            return toDoItems.Select(item => new ToDoItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Status = item.Status
            }).ToList();
        }
    }
}
