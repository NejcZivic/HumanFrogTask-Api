namespace HumanFrogApi.Models.Responses
{
    public class PostEventRegistrationsResponse
    {
        public required int EventId { get; set; }
        public required DateTime RegistrationDate { get; set; }
    }
}