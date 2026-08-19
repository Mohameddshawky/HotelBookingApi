using HotelBookingApi.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace HotelBookingApi.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", async ([FromBody] CreateOrUpdateBookingDto dto, CreateBookingHandler handler, CancellationToken ct) => 
        {
            var result = await handler.Handle(new CreateBookingCommand(dto), ct);
            return Results.CreatedAtRoute("GetBookingById", new { id = result.Id }, result);
        })
        .WithName("CreateBooking")
        .WithTags("Bookings");
    }
}
