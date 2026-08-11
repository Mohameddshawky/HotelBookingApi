using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestsController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _guestService.GetByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _guestService.GetByEmailAsync(email, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateGuestDto dto, CancellationToken cancellationToken)
    {
        var id = await _guestService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrUpdateGuestDto dto, CancellationToken cancellationToken)
    {
        await _guestService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}/bookings")]
    public async Task<IActionResult> GetBookingHistory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _guestService.GetBookingHistoryAsync(id, cancellationToken);
        return Ok(result);
    }
}
