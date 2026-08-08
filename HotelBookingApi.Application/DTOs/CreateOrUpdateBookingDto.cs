using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateBookingDto
{
    [Required]
    public Guid GuestId { get; set; }

    [Required]
    public Guid RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }
}
