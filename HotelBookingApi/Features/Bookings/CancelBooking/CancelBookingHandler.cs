using HotelBookingApi.Application.Interfaces.Notifications;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Domain.Enums;
using HotelBookingApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.CancelBooking;

public class CancelBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEnumerable<INotificationStrategy> _notificationStrategies;

    public CancelBookingHandler(IUnitOfWork unitOfWork, IEnumerable<INotificationStrategy> notificationStrategies)
    {
        _unitOfWork = unitOfWork;
        _notificationStrategies = notificationStrategies;
    }

    public async Task Handle(CancelBookingCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(command.Id);
            if (booking == null) throw new NotFoundException("Booking not found");

            booking.Cancel();
            
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

            var guest = await _unitOfWork.Guests.GetByIdAsync(booking.GuestId);
            if (guest != null)
            {
                foreach (var strategy in _notificationStrategies)
                {
                    await strategy.SendBookingCancelledAsync(booking, guest, cancellationToken);
                }
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
