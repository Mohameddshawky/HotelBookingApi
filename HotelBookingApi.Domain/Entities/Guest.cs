using System.Collections.Generic;
using HotelBookingApi.Domain.Common;

namespace HotelBookingApi.Domain.Entities;

public class Guest : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
