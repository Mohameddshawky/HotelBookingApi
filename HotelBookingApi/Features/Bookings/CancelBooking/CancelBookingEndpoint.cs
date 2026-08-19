using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/bookings/{id:guid}/cancel", [Authorize(Roles = "Staff")] async (Guid id, CancelBookingHandler handler, CancellationToken ct) => 
        {
            await handler.Handle(new CancelBookingCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("CancelBooking")
        .WithTags("Bookings");
    }
}
