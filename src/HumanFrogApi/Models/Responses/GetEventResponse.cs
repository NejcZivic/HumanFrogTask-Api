namespace HumanFrogApi.Models.Responses
{
    public class GetEventResponse
    {
        public required int Id { get; set; }
        public required string Naziv { get; set; }
        public required DateTime DatumInUra { get; set; }
        public required string Lokacija { get; set; }
        public required int Kapaciteta { get; set; }
        public required int ProstaMesta { get; set; }
    }
}