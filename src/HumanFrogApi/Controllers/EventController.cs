using Microsoft.AspNetCore.Mvc;
using HumanFrogApi.Models.Requests;
using HumanFrogApi.Models.Responses;
using HumanFrogApi.Services;
using HumanFrogApi.Services.EventService;

namespace HumanFrogApi.Controllers;

[ApiController]
[Route("events")]
[Produces("application/json")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(EventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEvents([FromQuery] EventsRequest query)
    {
        if (query.From.HasValue && query.To.HasValue && query.From > query.To)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "From date cannot be later than To date.");
        }

        var events = await _eventService.GetEventsAsync(query.From, query.To);
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEvent(int id)
    {
        var result = await _eventService.GetEventAsync(id);
        return result.IsSuccess ? Ok(result.Value) : ErrorResult(result.Error!);
    }

    [HttpPost("{id:int}/registrations")]
    [ProducesResponseType(typeof(PostEventRegistrationsResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostEventRegistration(int id, [FromBody] PostEventRegistrationsRequest request)
    {
        var result = await _eventService.RegisterForEventAsync(id, request.Email, request.Name, request.Surname);
        if (!result.IsSuccess)
        {
            return ErrorResult(result.Error!);
        }

        return CreatedAtAction(nameof(GetEvent), new { id }, result.Value);
    }

    private ObjectResult ErrorResult(ServiceError error)
    {
        var statusCode = error.Type switch
        {
            ServiceErrorType.NotFound => StatusCodes.Status404NotFound,
            ServiceErrorType.Validation => StatusCodes.Status400BadRequest,
            ServiceErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, detail: error.Message);
    }
}
