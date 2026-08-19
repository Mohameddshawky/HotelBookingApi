using HotelBookingApi.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.CheckInBooking;

public class CheckInBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckInBookingHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CheckInBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(command.Id);
        if (booking == null) throw new Exception("Booking not found");

        var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId);
        if (room != null)
        {
            booking.Room = room;
        }

        booking.CheckIn();
        
        _unitOfWork.Bookings.Update(booking);
        if (room != null) _unitOfWork.Rooms.Update(room);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
