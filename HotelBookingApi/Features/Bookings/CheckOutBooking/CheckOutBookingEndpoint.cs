using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.CheckOutBooking;

public static class CheckOutBookingEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/bookings/{id:guid}/checkout", [Authorize(Roles = "Staff")] async (Guid id, CheckOutBookingHandler handler, CancellationToken ct) => 
        {
            await handler.Handle(new CheckOutBookingCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("CheckOutBooking")
        .WithTags("Bookings");
    }
}
