using System;

namespace HotelBookingApi.Application.DTOs;

public class SaveReviewDto
{
    public Guid BookingId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
