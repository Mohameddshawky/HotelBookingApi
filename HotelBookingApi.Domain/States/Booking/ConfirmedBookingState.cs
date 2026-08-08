using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.States.Booking;

public class ConfirmedBookingState : BookingState
{
    public override void CheckIn(Entities.Booking booking)
    {
        booking.Status = BookingStatus.CheckedIn;
        if (booking.Room != null)
        {
            booking.Room.Status = RoomStatus.Occupied;
        }
    }

    public override void Cancel(Entities.Booking booking)
    {
        booking.Status = BookingStatus.Cancelled;
    }
}
