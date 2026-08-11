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
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypesController(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _roomTypeService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roomTypeService.GetByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }
    [Authorize(Roles ="Staff")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateRoomTypeDto dto, CancellationToken cancellationToken)
    {
        var id = await _roomTypeService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrUpdateRoomTypeDto dto, CancellationToken cancellationToken)
    {
        await _roomTypeService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }
}
