using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Notifications;

public interface INotificationStrategy
{
    Task SendBookingConfirmedAsync(Booking booking, Guest guest, CancellationToken cancellationToken = default);
    Task SendBookingCancelledAsync(Booking booking, Guest guest, CancellationToken cancellationToken = default);
}
