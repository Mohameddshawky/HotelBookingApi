using System;

namespace HotelBookingApi.Application.DTOs;

public class AmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
