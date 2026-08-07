using System;
using System.Collections.Generic;
using HotelBookingApi.Domain.Common;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.Entities;

public class Room : BaseEntity
{
    public string Number { get; set; } = string.Empty;
    public Guid RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }
    public RoomStatus Status { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
