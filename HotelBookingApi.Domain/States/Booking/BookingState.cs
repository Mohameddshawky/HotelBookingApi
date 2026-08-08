using System;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.States.Booking;

public abstract class BookingState
{
    public virtual void Confirm(Entities.Booking booking)
    {
        throw new InvalidOperationException($"Cannot confirm a booking in the {booking.Status} state.");
    }

    public virtual void Cancel(Entities.Booking booking)
    {
        throw new InvalidOperationException($"Cannot cancel a booking in the {booking.Status} state.");
    }

    public virtual void CheckIn(Entities.Booking booking)
    {
        throw new InvalidOperationException($"Cannot check-in a booking in the {booking.Status} state.");
    }

    public virtual void CheckOut(Entities.Booking booking)
    {
        throw new InvalidOperationException($"Cannot check-out a booking in the {booking.Status} state.");
    }
}
