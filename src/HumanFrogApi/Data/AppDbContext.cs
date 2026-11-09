using Microsoft.EntityFrameworkCore;
using HumanFrogApi.Entities;

namespace HumanFrogApi.Data;

public class HumanFrogDbContext : DbContext
{
    public HumanFrogDbContext(DbContextOptions<HumanFrogDbContext> options) : base(options)
    {
    }

    public DbSet<Dogodek> Dogodki => Set<Dogodek>();
    public DbSet<Prijava> Prijave => Set<Prijava>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanFrogDbContext).Assembly);

        modelBuilder.Entity<Dogodek>().HasData(
            new Dogodek
            {
                Id = 1,
                Naziv = "Koncert",
                DatumInUra = new DateTime(2026, 12, 15, 20, 0, 0).ToUniversalTime(),
                Lokacija = "Ljubljana",
                Kapaciteta = 100
            },
            new Dogodek
            {
                Id = 2,
                Naziv = "Delavnica",
                DatumInUra = new DateTime(2026, 12, 20, 10, 0, 0).ToUniversalTime(),
                Lokacija = "Maribor",
                Kapaciteta = 30
            }
        );
    }
}