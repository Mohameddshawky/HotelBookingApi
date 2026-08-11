using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] Guid? roomTypeId = null, 
        CancellationToken cancellationToken = default)
    {
        var result = await _roomService.GetAllAsync(pageNumber, pageSize, roomTypeId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roomService.GetByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, CancellationToken cancellationToken)
    {
        var result = await _roomService.GetAvailableRoomsAsync(checkIn, checkOut, cancellationToken);
        return Ok(result);
    }
    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateRoomDto dto, CancellationToken cancellationToken)
    {
        var id = await _roomService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    [Authorize(Roles = "Staff")]

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await _roomService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }
}
