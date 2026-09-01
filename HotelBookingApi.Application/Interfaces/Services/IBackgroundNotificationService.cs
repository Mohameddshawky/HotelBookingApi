using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IBackgroundNotificationService
{
    Task SendConfirmationEmailAsync(Guid bookingId);
    Task SendCheckInReminderAsync(Guid bookingId);
}
