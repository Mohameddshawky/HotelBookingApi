using System;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Application.DTOs;

public class SaveRoomDto
{
    public string Number { get; set; } = string.Empty;
    public Guid RoomTypeId { get; set; }
    public RoomStatus Status { get; set; }
}
