using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Features.Queries;
using MediatR;

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
    }
}
