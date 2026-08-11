using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, CancellationToken cancellationToken)
    {
        var result = await _reportService.GetAvailableRoomsAsync(checkIn, checkOut, cancellationToken);
        return Ok(result);
    }

    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancyReport(CancellationToken cancellationToken)
    {
        var result = await _reportService.GetOccupancyReportAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("room-type-ratings")]
    public async Task<IActionResult> GetRoomTypeRatings(CancellationToken cancellationToken)
    {
        var result = await _reportService.GetRoomTypeRatingsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("guest-history/{guestId}")]
    public async Task<IActionResult> GetGuestBookingHistory(Guid guestId, CancellationToken cancellationToken)
    {
        var result = await _reportService.GetGuestBookingHistoryAsync(guestId, cancellationToken);
        return Ok(result);
    }
}
