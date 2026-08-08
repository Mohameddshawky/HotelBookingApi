using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.States.Booking;

public class PendingBookingState : BookingState
{
    public override void Confirm(Entities.Booking booking)
    {
        booking.Status = BookingStatus.Confirmed;
    }

    public override void Cancel(Entities.Booking booking)
    {
        booking.Status = BookingStatus.Cancelled;
    }
}
