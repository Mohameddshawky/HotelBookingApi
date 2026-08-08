using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateRoomTypeDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 1000000)]
    public decimal PricePerNight { get; set; }

    [Range(1, 20)]
    public int MaxGuests { get; set; }

    public List<Guid> AmenityIds { get; set; } = new();
}
