using HumanFrogApi.Models.Responses;

namespace HumanFrogApi.Services.EventService;

public interface IEventService
{
    Task<EventsResponse> GetEventsAsync(DateTime? from, DateTime? to);

    Task<ServiceResult<GetEventResponse>> GetEventAsync(int id);

    Task<ServiceResult<PostEventRegistrationsResponse>> RegisterForEventAsync(int eventId, string email, string name, string surname);
}
