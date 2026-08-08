using System;

namespace HotelBookingApi.Application.DTOs;

public class ReviewDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
