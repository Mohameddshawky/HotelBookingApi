using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Features.Bookings.CreateBooking;

public record CreateBookingCommand(CreateOrUpdateBookingDto Dto);
