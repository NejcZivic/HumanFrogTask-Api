using HumanFrogApi.Models.Responses;

namespace HumanFrogApi.Services.EventService;

public interface IEventService
{
    EventsResponse GetEvents(DateTime from, DateTime to);
}