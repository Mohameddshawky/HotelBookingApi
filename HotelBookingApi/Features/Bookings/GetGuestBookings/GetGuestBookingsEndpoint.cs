using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.GetGuestBookings;

public static class GetGuestBookingsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/guest/{guestId:guid}", async (Guid guestId, GetGuestBookingsHandler handler, CancellationToken ct) => 
        {
            var result = await handler.Handle(new GetGuestBookingsQuery(guestId), ct);
            return Results.Ok(result);
        })
        .WithName("GetGuestBookings")
        .WithTags("Bookings");
    }
}
