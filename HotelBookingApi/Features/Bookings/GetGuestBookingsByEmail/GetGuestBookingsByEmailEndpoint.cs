using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.GetGuestBookingsByEmail;

public static class GetGuestBookingsByEmailEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/guest/by-email/{email}", async (string email, GetGuestBookingsByEmailHandler handler, CancellationToken ct) => 
        {
            var result = await handler.Handle(new GetGuestBookingsByEmailQuery(email), ct);
            return Results.Ok(result);
        })
        .WithName("GetGuestBookingsByEmail")
        .WithTags("Bookings");
    }
}
