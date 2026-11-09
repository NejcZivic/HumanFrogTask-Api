using System.ComponentModel.DataAnnotations;

namespace HumanFrogApi.Models.Requests;

public class PostEventRegistrationsRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Surname { get; set; }
}