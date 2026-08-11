using System;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;
using HotelBookingApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var result = await _reviewService.GetByBookingIdAsync(bookingId, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateReviewDto dto, CancellationToken cancellationToken)
    {
        var id = await _reviewService.CreateAsync(dto, cancellationToken);
        return Ok(new { id });
    }
}
