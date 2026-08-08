using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.States.Booking;

public class CheckedInBookingState : BookingState
{
    public override void CheckOut(Entities.Booking booking)
    {
        booking.Status = BookingStatus.CheckedOut;
        if (booking.Room != null)
        {
            booking.Room.Status = RoomStatus.Available;
        }
    }
}
