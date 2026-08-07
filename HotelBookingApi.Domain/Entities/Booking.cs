using System;
using HotelBookingApi.Domain.Common;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid GuestId { get; set; }
    public Guest? Guest { get; set; }

    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    
    public Review? Review { get; set; }
}
