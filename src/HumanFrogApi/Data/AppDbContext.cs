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
    }
}