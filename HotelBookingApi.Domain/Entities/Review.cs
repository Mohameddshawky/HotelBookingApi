using System;
using HotelBookingApi.Domain.Common;

namespace HotelBookingApi.Domain.Entities;

public class Review : BaseEntity
{
    public Guid BookingId { get; set; }
    public Booking? Booking { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
