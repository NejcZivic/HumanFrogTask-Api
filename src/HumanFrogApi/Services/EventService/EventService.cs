using System.Data;
using Microsoft.EntityFrameworkCore;
using HumanFrogApi.Models.Responses;
using HumanFrogApi.Data;
using HumanFrogApi.Entities;

namespace HumanFrogApi.Services.EventService;

public class EventService : IEventService
{
    private readonly HumanFrogDbContext _context;

    public EventService(HumanFrogDbContext context)
    {
        _context = context;
    }

    public async Task<EventsResponse> GetEventsAsync(DateTime? from, DateTime? to)
    {
        var events = _context.Dogodki.AsQueryable();

        if (from.HasValue)
        {
            var fromUtc = ToUtc(from.Value);
            events = events.Where(e => e.DatumInUra >= fromUtc);
        }
        if (to.HasValue)
        {
            var toUtc = ToUtc(to.Value);
            events = events.Where(e => e.DatumInUra <= toUtc);
        }

        var listEvents = await events
            .OrderBy(e => e.DatumInUra)
            .Select(e => new Event
            {
                Id = e.Id,
                Naziv = e.Naziv,
                DatumInUra = e.DatumInUra,
                Lokacija = e.Lokacija,
                Kapaciteta = e.Kapaciteta,
            })
            .ToListAsync();

        return new EventsResponse { Events = listEvents };
    }

    public async Task<ServiceResult<GetEventResponse>> GetEventAsync(int id)
    {
        var ev = await _context.Dogodki
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new GetEventResponse
            {
                Id = e.Id,
                Naziv = e.Naziv,
                DatumInUra = e.DatumInUra,
                Lokacija = e.Lokacija,
                Kapaciteta = e.Kapaciteta,
                ProstaMesta = e.Kapaciteta - e.Prijave.Count
            })
            .FirstOrDefaultAsync();

        if (ev == null)
        {
            return ServiceResult<GetEventResponse>.NotFound($"Event {id} was not found.");
        }

        return ServiceResult<GetEventResponse>.Success(ev);
    }

    public async Task<ServiceResult<PostEventRegistrationsResponse>> RegisterForEventAsync(int eventId, string email, string name, string surname)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        var eventEntity = await _context.Dogodki
            .FromSqlInterpolated($"SELECT * FROM dogodek WHERE id = {eventId} FOR UPDATE")
            .FirstOrDefaultAsync();

        if (eventEntity == null)
        {
            return ServiceResult<PostEventRegistrationsResponse>.NotFound($"Event {eventId} was not found.");
        }

        if (eventEntity.DatumInUra <= DateTime.UtcNow)
        {
            return ServiceResult<PostEventRegistrationsResponse>.Validation("Registration is closed because the event has already started.");
        }

        var registrationCount = await _context.Prijave.CountAsync(p => p.DogodekId == eventId);
        if (registrationCount >= eventEntity.Kapaciteta)
        {
            return ServiceResult<PostEventRegistrationsResponse>.Conflict("The event is fully booked.");
        }

        var alreadyRegistered = await _context.Prijave.AnyAsync(p => p.DogodekId == eventId && p.EPosta == normalizedEmail);
        if (alreadyRegistered)
        {
            return ServiceResult<PostEventRegistrationsResponse>.Conflict("This email is already registered for the event.");
        }

        var registration = new Prijava
        {
            DogodekId = eventId,
            EPosta = normalizedEmail,
            ImeInPriimek = $"{name.Trim()} {surname.Trim()}",
            CasPrijave = DateTime.UtcNow
        };

        _context.Prijave.Add(registration);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return ServiceResult<PostEventRegistrationsResponse>.Success(new PostEventRegistrationsResponse
        {
            EventId = eventId,
            RegistrationDate = registration.CasPrijave
        });
    }

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
