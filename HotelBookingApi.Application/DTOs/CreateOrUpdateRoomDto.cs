using System;
using System.ComponentModel.DataAnnotations;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Application.DTOs;

public class CreateOrUpdateRoomDto
{
    [Required]
    [StringLength(20)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public Guid RoomTypeId { get; set; }

    [Required]
    public RoomStatus Status { get; set; }
}
