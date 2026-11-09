using Microsoft.EntityFrameworkCore;
using HumanFrogApi.Entities;

namespace HumanFrogApi.Data;

public static class DbSeeder
{
    private enum SeedState
    {
        Past,
        FullyBooked,
        Open
    }

    private record SeedEvent(string Naziv, string Lokacija, int DaysFromNow, int Hour, int Kapaciteta, SeedState State, int Registrations);

    private static readonly SeedEvent[] Events =
    [
        new("Jazz večer", "Ljubljana", -60, 20, 50, SeedState.Past, 42),
        new("Delavnica fotografije", "Kranj", -45, 10, 15, SeedState.Past, 15),
        new("Tek za zdravje", "Celje", -30, 9, 200, SeedState.Past, 120),
        new("Predavanje o umetni inteligenci", "Maribor", -14, 18, 80, SeedState.Past, 65),
        new("Degustacija vin", "Koper", -7, 19, 25, SeedState.Past, 25),
        new("Knjižni klub", "Novo mesto", -2, 17, 20, SeedState.Past, 8),

        new("Stand-up komedija", "Ljubljana", 5, 21, 10, SeedState.FullyBooked, 10),
        new("Tečaj kuhanja", "Ptuj", 9, 16, 8, SeedState.FullyBooked, 8),
        new("Joga v parku", "Bled", 12, 8, 12, SeedState.FullyBooked, 12),
        new("Delavnica keramike", "Škofja Loka", 18, 14, 6, SeedState.FullyBooked, 6),
        new("Večer družabnih iger", "Velenje", 25, 19, 16, SeedState.FullyBooked, 16),
        new("Koncert klasične glasbe", "Piran", 40, 20, 20, SeedState.FullyBooked, 20),

        new("Hackathon", "Ljubljana", 7, 9, 60, SeedState.Open, 35),
        new("Pohod na Triglav", "Mojstrana", 14, 6, 30, SeedState.Open, 12),
        new("Filmski maraton", "Maribor", 20, 18, 100, SeedState.Open, 0),
        new("Konferenca o razvoju programske opreme", "Ljubljana", 30, 9, 300, SeedState.Open, 150),
        new("Delavnica slikanja", "Nova Gorica", 35, 15, 20, SeedState.Open, 19),
        new("Kolesarski izlet", "Kamnik", 45, 10, 40, SeedState.Open, 5),
        new("Novoletni sejem", "Celje", 50, 12, 500, SeedState.Open, 210),
        new("Tečaj plesa", "Kranj", 60, 19, 24, SeedState.Open, 10),
    ];

    private static readonly string[] FirstNames =
        ["Ana", "Luka", "Maja", "Nejc", "Eva", "Jan", "Nina", "Tim", "Sara", "Žiga", "Lara", "Matej", "Tina", "Rok", "Petra"];

    private static readonly string[] LastNames =
        ["Novak", "Horvat", "Kovačič", "Krajnc", "Zupančič", "Potočnik", "Mlakar", "Kos", "Vidmar", "Golob", "Turk", "Kralj"];

    public static async Task SeedAsync(HumanFrogDbContext context)
    {
        var seedNames = Events.Select(e => e.Naziv).ToList();
        if (await context.Dogodki.AnyAsync(e => seedNames.Contains(e.Naziv)))
        {
            return;
        }

        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval(pg_get_serial_sequence('dogodek', 'id'), COALESCE((SELECT MAX(id) FROM dogodek), 0) + 1, false)");

        var today = DateTime.UtcNow.Date;

        foreach (var seed in Events)
        {
            var datumInUra = today.AddDays(seed.DaysFromNow).AddHours(seed.Hour);

            var dogodek = new Dogodek
            {
                Naziv = seed.Naziv,
                DatumInUra = datumInUra,
                Lokacija = seed.Lokacija,
                Kapaciteta = seed.Kapaciteta,
                Prijave = CreateRegistrations(seed, datumInUra)
            };

            context.Dogodki.Add(dogodek);
        }

        await context.SaveChangesAsync();
    }

    private static List<Prijava> CreateRegistrations(SeedEvent seed, DateTime datumInUra)
    {
        var latest = datumInUra < DateTime.UtcNow ? datumInUra.AddHours(-1) : DateTime.UtcNow;
        var earliest = datumInUra.AddDays(-30);

        return [.. Enumerable.Range(0, seed.Registrations).Select(i =>
        {
            var firstName = FirstNames[i % FirstNames.Length];
            var lastName = LastNames[i / FirstNames.Length % LastNames.Length];
            var progress = seed.Registrations == 1 ? 0 : (double)i / (seed.Registrations - 1);

            return new Prijava
            {
                ImeInPriimek = $"{firstName} {lastName}",
                EPosta = $"{ToAscii(firstName)}.{ToAscii(lastName)}.{i}@example.com".ToLowerInvariant(),
                CasPrijave = earliest + (latest - earliest) * progress
            };
        })];
    }

    private static string ToAscii(string value) => value
        .Replace("č", "c").Replace("Č", "C")
        .Replace("š", "s").Replace("Š", "S")
        .Replace("ž", "z").Replace("Ž", "Z");
}
