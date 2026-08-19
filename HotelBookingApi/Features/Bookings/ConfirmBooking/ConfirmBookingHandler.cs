using HotelBookingApi.Application.Interfaces.Notifications;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Domain.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.ConfirmBooking;

public class ConfirmBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEnumerable<INotificationStrategy> _notificationStrategies;

    public ConfirmBookingHandler(IUnitOfWork unitOfWork, IEnumerable<INotificationStrategy> notificationStrategies)
    {
        _unitOfWork = unitOfWork;
        _notificationStrategies = notificationStrategies;
    }

    public async Task Handle(ConfirmBookingCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(command.Id);
            if (booking == null) throw new NotFoundException("Booking not found");

            booking.Confirm();
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var guest = await _unitOfWork.Guests.GetByIdAsync(booking.GuestId);
            if (guest != null)
            {
                foreach (var strategy in _notificationStrategies)
                {
                    await strategy.SendBookingConfirmedAsync(booking, guest, cancellationToken);
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
