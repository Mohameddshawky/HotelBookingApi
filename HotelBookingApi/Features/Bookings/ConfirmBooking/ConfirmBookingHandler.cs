using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Domain.Exceptions;
using Hangfire;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Features.Bookings.ConfirmBooking;

public class ConfirmBookingHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ConfirmBookingHandler(IUnitOfWork unitOfWork, IBackgroundJobClient backgroundJobClient)
    {
        _unitOfWork = unitOfWork;
        _backgroundJobClient = backgroundJobClient;
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

            // Enqueue immediate confirmation email
            _backgroundJobClient.Enqueue<IBackgroundNotificationService>(service => service.SendConfirmationEmailAsync(booking.Id));

            // Schedule check-in reminder for 30 seconds from now (for testing Gotcha #2)
            _backgroundJobClient.Schedule<IBackgroundNotificationService>(service => service.SendCheckInReminderAsync(booking.Id), TimeSpan.FromSeconds(30));

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
