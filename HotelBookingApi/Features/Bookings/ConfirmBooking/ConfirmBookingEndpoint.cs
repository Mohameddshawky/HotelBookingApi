using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.ConfirmBooking;

public static class ConfirmBookingEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/bookings/{id:guid}/confirm", [Authorize(Roles = "Staff")] async (Guid id, ConfirmBookingHandler handler, CancellationToken ct) => 
        {
            await handler.Handle(new ConfirmBookingCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("ConfirmBooking")
        .WithTags("Bookings");
    }
}
