using System;

namespace HotelBookingApi.Application.DTOs;

public class AvailableRoomDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public Guid RoomTypeId { get; set; }
    public decimal PricePerNight { get; set; }
}
