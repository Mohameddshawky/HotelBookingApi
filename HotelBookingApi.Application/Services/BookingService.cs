using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Domain.Enums;
using System.Data;

namespace HotelBookingApi.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookingDetailsDto> CreateAsync(CreateOrUpdateBookingDto dto, CancellationToken cancellationToken = default)
    {
        // ATOMIC UPDATE FOR CREATE BOOKING
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var isOverlapping = await _unitOfWork.Bookings.HasOverlappingBookingAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (isOverlapping)
            {
                throw new Exception("Room is already booked for the selected dates.");
            }

            var room = await _unitOfWork.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null || room.Status == RoomStatus.Maintenance)
            {
                throw new Exception("Room is not available.");
            }

            var booking = _mapper.Map<Booking>(dto);
            booking.Status = BookingStatus.Pending;
            
            // Assuming RoomType is loaded to get price, but since we map price from RoomType we might need to load it
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(room.RoomTypeId);
            if (roomType != null)
            {
                var days = (dto.CheckOutDate - dto.CheckInDate).Days;
                booking.TotalPrice = days * roomType.PricePerNight;
            }

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return _mapper.Map<BookingDetailsDto>(booking);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<BookingDetailsDto?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        return booking == null ? null : _mapper.Map<BookingDetailsDto>(booking);
    }

    public async Task<IEnumerable<BookingHistoryDto>> GetGuestBookingsAsync(Guid guestId, CancellationToken cancellationToken = default)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByGuestIdAsync(guestId);
        return _mapper.Map<IEnumerable<BookingHistoryDto>>(bookings);
    }

    public async Task ConfirmAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        if (booking == null) throw new Exception("Booking not found");

        booking.Confirm(); // Uses State Pattern
        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        if (booking == null) throw new Exception("Booking not found");

        booking.Cancel(); // Uses State Pattern
        
        // If they cancelled a checked-in booking (which shouldn't happen per state pattern, but if they did)
        if (booking.RoomId != Guid.Empty)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId);
            if (room != null && room.Status == RoomStatus.Occupied)
            {
                room.Status = RoomStatus.Available;
                _unitOfWork.Rooms.Update(room);
            }
        }

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CheckInAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        if (booking == null) throw new Exception("Booking not found");

        var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId);
        if (room != null)
        {
            booking.Room = room;
        }

        booking.CheckIn(); // Uses State Pattern
        
        _unitOfWork.Bookings.Update(booking);
        if (room != null) _unitOfWork.Rooms.Update(room);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CheckOutAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        if (booking == null) throw new Exception("Booking not found");

        var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId);
        if (room != null)
        {
            booking.Room = room;
        }

        booking.CheckOut(); // Uses State Pattern

        _unitOfWork.Bookings.Update(booking);
        if (room != null) _unitOfWork.Rooms.Update(room);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
