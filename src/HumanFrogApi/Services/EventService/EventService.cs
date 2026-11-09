namespace HumanFrogApi.Services.EventService;
using HumanFrogApi.Models.Responses;
using HumanFrogApi.Data;
public class EventService: IEventService
{
    private readonly HumanFrogDbContext _context;

    public EventService(HumanFrogDbContext context)
    {
        _context = context;
    }

    public EventsResponse GetEvents(DateTime from, DateTime to)
    {
        var events = _context.Dogodki.AsQueryable();

        if (from != DateTime.MinValue)
        {
            events = events.Where(e => e.DatumInUra >= from);
        }
        if (to != DateTime.MinValue)
        {
            events = events.Where(e => e.DatumInUra <= to);
        }

        var listEvents = events.ToList();

        return new EventsResponse
        {
            Events = [.. listEvents.Select(e => new Event
            {
                Id = e.Id,
                Naziv = e.Naziv,
                DatumInUra = e.DatumInUra,
                Lokacija = e.Lokacija,
                Kapaciteta = e.Kapaciteta,
            })]
        };
    }
}