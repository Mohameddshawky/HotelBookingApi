using System;

namespace HotelBookingApi.Application.DTOs;

public class RoomTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
}
