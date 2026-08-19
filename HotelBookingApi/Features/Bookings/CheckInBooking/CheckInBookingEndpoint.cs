using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.CheckInBooking;

public static class CheckInBookingEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/bookings/{id:guid}/checkin", [Authorize(Roles = "Staff")] async (Guid id, CheckInBookingHandler handler, CancellationToken ct) => 
        {
            await handler.Handle(new CheckInBookingCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("CheckInBooking")
        .WithTags("Bookings");
    }
}
