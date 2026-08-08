using System;

namespace HotelBookingApi.Application.DTOs;

public class SaveBookingDto
{
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
