using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Features.Queries;
using MediatR;
using ToDoList.Application.Features.Commands;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetToDoList")]
        public async Task<IActionResult> Get([FromQuery] GetToDoListQuery query)
        {           
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = "AddToDoItem")]
        public async Task<IActionResult> Post([FromBody] AddToDoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}", Name = "DeleteToDoItem")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteToDoItemCommand(id));
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
