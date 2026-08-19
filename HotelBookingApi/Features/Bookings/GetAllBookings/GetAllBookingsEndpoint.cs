using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.GetAllBookings;

public static class GetAllBookingsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings", [Authorize(Roles = "Staff")] async (GetAllBookingsHandler handler, CancellationToken ct) => 
        {
            var result = await handler.Handle(new GetAllBookingsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllBookings")
        .WithTags("Bookings");
    }
}
