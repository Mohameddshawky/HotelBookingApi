using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", async (Guid id, GetBookingByIdHandler handler, CancellationToken ct) => 
        {
            var result = await handler.Handle(new GetBookingByIdQuery(id), ct);
            return result == null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetBookingById")
        .WithTags("Bookings");
    }
}
