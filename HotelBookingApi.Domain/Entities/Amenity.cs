using System.Collections.Generic;
using HotelBookingApi.Domain.Common;

namespace HotelBookingApi.Domain.Entities;

public class Amenity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; } = new List<RoomTypeAmenity>();
}
