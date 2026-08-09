using System;

namespace HotelBookingApi.Application.DTOs.Auth;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? ErrorMessage { get; set; }
}
