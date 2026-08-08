using System;
using System.Collections.Generic;

namespace HotelBookingApi.Application.DTOs;

public class SaveRoomTypeDto
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
    public List<Guid> AmenityIds { get; set; } = new();
}
