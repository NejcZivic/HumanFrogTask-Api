using Microsoft.AspNetCore.Mvc;
using HumanFrogApi.Models.Requests;
using HumanFrogApi.Models.Responses;
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
    [ProducesResponseType(typeof(IEnumerable<EventsResponse>), StatusCodes.Status200OK)]
    public IActionResult GetEvents([FromQuery] EventsRequest query)
    {
        var from = query.From;
        var to = query.To;

        if (from > to)
        {
            return BadRequest("From date cannot be later than To date.");
        }

        var events = _eventService.GetEvents(from, to);
        return Ok(events);
    }
}