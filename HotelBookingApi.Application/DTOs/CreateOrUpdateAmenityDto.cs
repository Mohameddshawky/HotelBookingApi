using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateAmenityDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
