using System;
using HotelBookingApi.Domain.Common;
using HotelBookingApi.Domain.Enums;

namespace HotelBookingApi.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid GuestId { get; set; }
    public Guest? Guest { get; set; }

    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    
    public Review? Review { get; }

    public HotelBookingApi.Domain.States.Booking.BookingState CurrentState
    {
        get
        {
            return Status switch
            {
                BookingStatus.Pending => new HotelBookingApi.Domain.States.Booking.PendingBookingState(),
                BookingStatus.Confirmed => new HotelBookingApi.Domain.States.Booking.ConfirmedBookingState(),
                BookingStatus.CheckedIn => new HotelBookingApi.Domain.States.Booking.CheckedInBookingState(),
                BookingStatus.CheckedOut => new HotelBookingApi.Domain.States.Booking.CheckedOutBookingState(),
                BookingStatus.Cancelled => new HotelBookingApi.Domain.States.Booking.CancelledBookingState(),
                _ => throw new NotImplementedException()
            };
        }
    }

    public void Confirm() => CurrentState.Confirm(this);
    public void Cancel() => CurrentState.Cancel(this);
    public void CheckIn() => CurrentState.CheckIn(this);
    public void CheckOut() => CurrentState.CheckOut(this);
}
