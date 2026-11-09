using System.ComponentModel.DataAnnotations.Schema;

namespace HumanFrogApi.Entities;

[Table("dogodek")]
public class Dogodek
{
    [Column("id")]
    public int Id { get; set; }

    [Column("naziv")]
    public required string Naziv { get; set; }

    [Column("datum_in_ura")]
    public DateTime DatumInUra { get; set; }

    [Column("lokacija")]
    public required string Lokacija { get; set; }

    [Column("kapaciteta")]
    public int Kapaciteta { get; set; }

    public List<Prijava> Prijave { get; set; } = new();
}