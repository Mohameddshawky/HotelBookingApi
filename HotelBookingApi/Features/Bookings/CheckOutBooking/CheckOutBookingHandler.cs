using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Domain.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.CheckOutBooking;

public class CheckOutBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckOutBookingHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CheckOutBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(command.Id);
        if (booking == null) throw new NotFoundException("Booking not found");

        var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId);
        if (room != null)
        {
            booking.Room = room;
        }

        booking.CheckOut();

        _unitOfWork.Bookings.Update(booking);
        if (room != null) _unitOfWork.Rooms.Update(room);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
