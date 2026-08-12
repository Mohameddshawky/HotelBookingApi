using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateBookingDto
{
    [Required]
    [EmailAddress]
    public string GuestEmail { get; set; } = string.Empty;

    [Required]
    public string GuestFirstName { get; set; } = string.Empty;

    [Required]
    public string GuestLastName { get; set; } = string.Empty;

    [Required]
    public string GuestPhone { get; set; } = string.Empty;

    [Required]
    public Guid RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }
}
