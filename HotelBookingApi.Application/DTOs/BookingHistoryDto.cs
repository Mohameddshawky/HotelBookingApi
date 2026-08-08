using System;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Application.DTOs;

public class BookingHistoryDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
}
