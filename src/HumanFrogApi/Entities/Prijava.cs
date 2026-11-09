using System.ComponentModel.DataAnnotations.Schema;

namespace HumanFrogApi.Entities;

[Table("prijava")]
public class Prijava
{
    [Column("id")]
    public int Id { get; set; }

    [Column("dogodek_id")]
    public int DogodekId { get; set; }

    [Column("ime_in_priimek")]
    public required string ImeInPriimek { get; set; }

    [Column("e_posta")]
    public required string EPosta { get; set; }

    [Column("cas_prijave")]
    public DateTime CasPrijave { get; set; }

    [ForeignKey(nameof(DogodekId))]
    public Dogodek? Dogodek { get; set; }
}