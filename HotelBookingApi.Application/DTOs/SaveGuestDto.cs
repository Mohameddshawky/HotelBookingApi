using System;

namespace HotelBookingApi.Application.DTOs;

public class SaveGuestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
