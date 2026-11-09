namespace HumanFrogApi.Models.Responses
{
    public class EventsResponse
    {
        public List<Event> Events { get; set; } = [];
    }

    public class Event
    {
        public required int Id { get; set; }
        public required string Naziv { get; set; }
        public required DateTime DatumInUra { get; set; }
        public required string Lokacija { get; set; }
        public required int Kapaciteta { get; set; }
    }
}