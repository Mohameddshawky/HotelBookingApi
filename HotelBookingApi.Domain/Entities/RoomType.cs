using System.Collections.Generic;
using HotelBookingApi.Domain.Common;

namespace HotelBookingApi.Domain.Entities;

public class RoomType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; } = new List<RoomTypeAmenity>();
}
