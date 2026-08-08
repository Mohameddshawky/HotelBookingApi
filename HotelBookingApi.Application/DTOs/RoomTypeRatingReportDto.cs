using System;

namespace HotelBookingApi.Application.DTOs;

public class RoomTypeRatingReportDto
{
    public Guid RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public double AverageRating { get; set; }
}
