using MediatR;
using ToDoList.Infrastructure;

namespace ToDoList.Application.Features.Commands
{
    public sealed record DeleteToDoItemCommand(Guid Id) : IRequest<bool>;

    public sealed class DeleteToDoItemCommandHandler : IRequestHandler<DeleteToDoItemCommand, bool>
    {
        private readonly IToDoListRepository _repo;

        public DeleteToDoItemCommandHandler(IToDoListRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteToDoItemAsync(request.Id, cancellationToken);
        }
    }
}
