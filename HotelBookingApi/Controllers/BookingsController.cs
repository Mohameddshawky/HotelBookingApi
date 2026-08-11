using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateBookingDto dto, CancellationToken cancellationToken)
    {
        var result = await _bookingService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("guest/{guestId}")]
    public async Task<IActionResult> GetGuestBookings(Guid guestId, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetGuestBookingsAsync(guestId, cancellationToken);
        return Ok(result);
    }
    [Authorize(Roles = "Staff")]

    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        await _bookingService.ConfirmAsync(id, cancellationToken);
        return NoContent();
    }
    [Authorize(Roles = "Staff")]

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await _bookingService.CancelAsync(id, cancellationToken);
        return NoContent();
    }
    [Authorize(Roles = "Staff")]

    [HttpPut("{id}/checkin")]
    public async Task<IActionResult> CheckIn(Guid id, CancellationToken cancellationToken)
    {
        await _bookingService.CheckInAsync(id, cancellationToken);
        return NoContent();
    }
    [Authorize(Roles = "Staff")]

    [HttpPut("{id}/checkout")]
    public async Task<IActionResult> CheckOut(Guid id, CancellationToken cancellationToken)
    {
        await _bookingService.CheckOutAsync(id, cancellationToken);
        return NoContent();
    }
}
