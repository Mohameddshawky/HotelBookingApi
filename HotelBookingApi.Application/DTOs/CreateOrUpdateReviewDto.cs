using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateReviewDto
{
    [Required]
    public Guid BookingId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string Comment { get; set; } = string.Empty;
}
