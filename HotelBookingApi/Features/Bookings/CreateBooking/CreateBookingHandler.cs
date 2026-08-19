using AutoMapper;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Domain.Enums;
using HotelBookingApi.Domain.Exceptions;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.CreateBooking;

public class CreateBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookingHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookingDetailsDto> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var dto = command.Dto;
        
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var isOverlapping = await _unitOfWork.Bookings.HasOverlappingBookingAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (isOverlapping)
            {
                throw new BadRequestException("Room is already booked for the selected dates.");
            }

            var room = await _unitOfWork.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null || room.Status == RoomStatus.Maintenance)
            {
                throw new BadRequestException("Room is not available.");
            }

            var guest = await _unitOfWork.Guests.GetByEmailAsync(dto.GuestEmail);
            if (guest == null)
            {
                guest = new Guest
                {
                    FullName = $"{dto.GuestFirstName} {dto.GuestLastName}".Trim(),
                    Email = dto.GuestEmail,
                    PhoneNumber = dto.GuestPhone
                };
                await _unitOfWork.Guests.AddAsync(guest);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var booking = new Booking
            {
                GuestId = guest.Id,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Status = BookingStatus.Pending
            };
            
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
}
